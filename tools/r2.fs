/// Minimal S3 client for Cloudflare R2 - just enough to PUT an object. Only one request
/// shape is needed, so SigV4 is signed by hand rather than pulling in the AWS SDK.
module FsBlog.R2

open System
open System.IO
open System.Net.Http
open System.Security.Cryptography
open System.Text

let private sha256Hex (bytes:byte[]) =
  use sha = SHA256.Create()
  sha.ComputeHash(bytes) |> Array.map (fun b -> b.ToString("x2")) |> String.concat ""

let private hmac (key:byte[]) (data:string) =
  use h = new HMACSHA256(key)
  h.ComputeHash(Encoding.UTF8.GetBytes data)

/// Percent-encode a path segment per RFC 3986, as SigV4 requires
let private uriEncode (s:string) =
  let unreserved c =
    (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') ||
    c = '-' || c = '_' || c = '.' || c = '~'
  let sb = StringBuilder()
  for b in Encoding.UTF8.GetBytes s do
    let c = char b
    if unreserved c then sb.Append(c) |> ignore
    else sb.Append('%').Append(b.ToString("X2")) |> ignore
  sb.ToString()

/// Bucket the calendar photos live in
let [<Literal>] Bucket = "tomasp-calendar"

type Credentials =
  { AccountId : string
    AccessKeyId : string
    SecretAccessKey : string }

/// Read credentials from the environment (loaded from a gitignored .env by build.sh)
let credentialsFromEnvironment () =
  let get name =
    match Environment.GetEnvironmentVariable(name:string) with
    | null | "" -> failwithf "Environment variable '%s' is not set (needed to upload to R2)" name
    | v -> v
  { AccountId = get "R2_ACCOUNT_ID"
    AccessKeyId = get "R2_ACCESS_KEY_ID"
    SecretAccessKey = get "R2_SECRET_ACCESS_KEY" }

let private client = lazy (new HttpClient())

/// Upload one object. `key` is the path inside the bucket, e.g. "calendar/2026/january.jpg".
let put (cred:Credentials) (key:string) (contentType:string) (cacheControl:string) (body:byte[]) =
  let host = sprintf "%s.r2.cloudflarestorage.com" cred.AccountId
  let path = "/" + Bucket + "/" + (key.Split('/') |> Array.map uriEncode |> String.concat "/")
  let now = DateTime.UtcNow
  let amzDate = now.ToString("yyyyMMdd'T'HHmmss'Z'")
  let dateStamp = now.ToString("yyyyMMdd")
  let payloadHash = sha256Hex body
  // R2 ignores the region; "auto" is what Cloudflare documents
  let scope = sprintf "%s/auto/s3/aws4_request" dateStamp
  let signedHeaders = "host;x-amz-content-sha256;x-amz-date"

  let canonicalRequest =
    String.concat "\n"
      [ "PUT"; path; ""
        sprintf "host:%s" host
        sprintf "x-amz-content-sha256:%s" payloadHash
        sprintf "x-amz-date:%s" amzDate
        ""
        signedHeaders
        payloadHash ]

  let stringToSign =
    String.concat "\n"
      [ "AWS4-HMAC-SHA256"; amzDate; scope
        sha256Hex (Encoding.UTF8.GetBytes canonicalRequest) ]

  let signature =
    let kDate = hmac (Encoding.UTF8.GetBytes("AWS4" + cred.SecretAccessKey)) dateStamp
    let kRegion = hmac kDate "auto"
    let kService = hmac kRegion "s3"
    let kSigning = hmac kService "aws4_request"
    hmac kSigning stringToSign |> Array.map (fun b -> b.ToString("x2")) |> String.concat ""

  use req = new HttpRequestMessage(HttpMethod.Put, "https://" + host + path)
  req.Content <- new ByteArrayContent(body)
  req.Content.Headers.ContentType <- Headers.MediaTypeHeaderValue(contentType)
  // Cache-Control is a general header, so it does not belong on HttpContent
  req.Headers.TryAddWithoutValidation("Cache-Control", cacheControl) |> ignore
  req.Headers.TryAddWithoutValidation("x-amz-content-sha256", payloadHash) |> ignore
  req.Headers.TryAddWithoutValidation("x-amz-date", amzDate) |> ignore
  req.Headers.TryAddWithoutValidation("Authorization",
    sprintf "AWS4-HMAC-SHA256 Credential=%s/%s, SignedHeaders=%s, Signature=%s"
      cred.AccessKeyId scope signedHeaders signature) |> ignore

  let res = client.Value.Send(req)
  if not res.IsSuccessStatusCode then
    let body = res.Content.ReadAsStringAsync().Result
    failwithf "Upload of '%s' failed: %d %s\n%s" key (int res.StatusCode) res.ReasonPhrase body

let putFile (cred:Credentials) (key:string) (contentType:string) (cacheControl:string) (file:string) =
  put cred key contentType cacheControl (File.ReadAllBytes file)
