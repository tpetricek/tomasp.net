(**

Advent Art：北斎の絵を生成する
================================

 - date: 2013-12-24T03:58:37.5115170+00:00
 - description: この記事は毎年日本のF#コミュニティで主催されているF# Advent Calendarへの寄稿です。それゆえに、日本文化にインスパイアされたものになっています。ここではFreebaseにある日本の画家一覧を参照して、フラクタルとFunScript（！）を組み合わせることで絵画の一部の再生成を試みています。
 - layout: post
 - tags: f#,art,fractals,funscript,f# data
 - title: Advent Art：北斎の絵を生成する
 - url: 2013/japan-advent-art

--------------------------------------------------------------------------------
 - standalone

<div id="myModal" class="reveal-modal xlarge" data-reveal>
  <iframe src="http://tomasp.net/blog/2013/japan-advent-art/hokusai.html" style="width:100%; height:850px;border-style:none;"></iframe>
  <a class="close-reveal-modal">&#215;</a>
</div>

<div class="rdecor" style="text-align:center">
<a href="#" data-reveal-id="myModal" style="text-align:center">
<img src="http://tomasp.net/blog/2013/japan-advent-art/hokusai_sm.jpg" style="margin-bottom:10px;border:4px solid black" title="神奈川沖浪裏 (The Great Wave off Kanagawa)" /><br />
<small style="font-size:90%">実際の結果を見るにはここをクリック！</small>
</a>
</div>

ここ数年、日本のF# コミュニティは「F# Advent Calendar」というイベントを開催しています
([2010年](http://atnd.org/events/10685)、
[2011年](http://partake.in/events/1c24993a-c475-4fc2-bca4-7a1cd2f81869)、
[2012年](http://atnd.org/events/33927)、
そして [今年](http://connpass.com/event/3935/))。
これはadvent dayごとに1人ずつ、F#に関連した何かしら興味深い記事を作成するというものです。
私は去年からTwitterでadvent calendarをチェックしていて、
今年からは私も参加しようと思い、記事を書きたいと申し出ました。
そうしたところ、数名の方からの協力を得ることができました。
[@@igeta](https://twitter.com/igeta) には参加手続きの諸々とレビューを、
[@@yukitos](http://twitter.com/yukitos) にはこの記事の翻訳を、そして
[@@gab_km](http://twitter.com/gab_km) には翻訳のレビューをしていただきました。
ありがとう！

けれども何についての記事を書くのがよいのでしょう？
過去一年にわたって、F#コミュニティで開発されているF#のオープンソースライブラリやプロジェクトを
いくつか紹介できるような記事がよさそうです。
それと同時に、日本に関連のあるトピックが何かないものでしょうか？
少し考えてみたところ、以下のようなプランを思いつきました：

 * まず、日本の絵画について学ぶために [F# Data][fsdata] ライブラリと [Freebase](http://www.freebase.com) を組み合わせて使う。
   このライブラリにはいまや [日本語ドキュメント][fsdatajp] があり、作成してくれた [@@yukitos](https://twitter.com/yukitos) に感謝しています。

 * そして絵画作品を1つ選択して、F#でその作品を再生成する。
   私の絵画スキルでは到底無理なのですが、試してみることはできます :-)

 * 最後に、 [FunScriptプロジェクト](http://funscript.info) を使って
   F#コードをJavaScriptに変換します。
   そうすると純粋なHTML Webアプリケーションとして実行できるようになり、
   携帯電話やその他のデバイスでも動作するようになります。

--------------------------------------------------------------------------------


<div id="myModal" class="reveal-modal xlarge" data-reveal>
  <iframe src="hokusai.html" style="width:100%; height:850px;border-style:none;"></iframe>
  <a class="close-reveal-modal">&#215;</a>
</div>

<div class="rdecor" style="text-align:center">
<a href="#" data-reveal-id="myModal" style="text-align:center">
<img src="hokusai_sm.jpg" style="margin-bottom:10px;border:4px solid black" title="神奈川沖浪裏 (The Great Wave off Kanagawa)" /><br />
<small style="font-size:90%">実際の結果を見るにはここをクリック！</small>
</a>
</div>

ここ数年、日本のF# コミュニティは「F# Advent Calendar」というイベントを開催しています
([2010年](http://atnd.org/events/10685)、
[2011年](http://partake.in/events/1c24993a-c475-4fc2-bca4-7a1cd2f81869)、
[2012年](http://atnd.org/events/33927)、
そして [今年](http://connpass.com/event/3935/))。
これはadvent dayごとに1人ずつ、F#に関連した何かしら興味深い記事を作成するというものです。
私は去年からTwitterでadvent calendarをチェックしていて、
今年からは私も参加しようと思い、記事を書きたいと申し出ました。
そうしたところ、数名の方からの協力を得ることができました。
[@@igeta](https://twitter.com/igeta) には参加手続きの諸々とレビューを、
[@@yukitos](http://twitter.com/yukitos) にはこの記事の翻訳を、そして
[@@gab_km](http://twitter.com/gab_km) には翻訳のレビューをしていただきました。
ありがとう！

けれども何についての記事を書くのがよいのでしょう？
過去一年にわたって、F#コミュニティで開発されているF#のオープンソースライブラリやプロジェクトを
いくつか紹介できるような記事がよさそうです。
それと同時に、日本に関連のあるトピックが何かないものでしょうか？
少し考えてみたところ、以下のようなプランを思いつきました：

 * まず、日本の絵画について学ぶために [F# Data][fsdata] ライブラリと [Freebase](http://www.freebase.com) を組み合わせて使う。
   このライブラリにはいまや [日本語ドキュメント][fsdatajp] があり、作成してくれた [@@yukitos](https://twitter.com/yukitos) に感謝しています。

 * そして絵画作品を1つ選択して、F#でその作品を再生成する。
   私の絵画スキルでは到底無理なのですが、試してみることはできます :-)

 * 最後に、 [FunScriptプロジェクト](http://funscript.info) を使って
   F#コードをJavaScriptに変換します。
   そうすると純粋なHTML Webアプリケーションとして実行できるようになり、
   携帯電話やその他のデバイスでも動作するようになります。

[fsdata]: http://fsharp.github.io/FSharp.Data/
[fsdatajp]: https://github.com/fsharp/FSharp.Data/blob/master/docs/content/ja/index.md

日本の芸術の探索
----------------

[Freebase](http://www.freebase.com) は主にWikipediaを情報源とする
体系的な情報を持ったオンラインのグラフデータベースです。
このデータベースには(政治、芸能などの)社会や(多種多様な)スポーツ、
(コンピュータや化学などの)サイエンス、そして芸術など、様々な分野に関する情報が
含まれています。

F# DataにあるFreebase用のF# 型プロバイダーを使用すると、
コードエディタ上から直接これらの情報すべてにアクセスできます。
この型プロバイダーを使うには、まず `FSharp.Data` のNuGetパッケージをインストールした後、
以下のようにしてアセンブリへの参照を追加します：
*)
#I "../packages/FSharp.Data.1.1.10/lib/net40"
#r "FSharp.Data.dll"

open FSharp.Data
open System.Linq
(**
`FSharp.Data` とは別に、いくつかLINQメソッドを使ってデータを探索します。
Freebaseへのアクセスには `FreebaseData` または `FreebaseDataProvider` を使います。
後者には大量のリクエストを発行するコードを作成する場合に必要になるAPIキーを指定します。
サンプルをきちんと動かすためには、登録を行う必要があるでしょう：
*)
type FreebaseData = FreebaseDataProvider<(*[omit:(自分のキーをこの位置に入力すること)]*)"AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w"(*[/omit]*)>
let fb = FreebaseData.GetDataContext()
(**
そして `fb.` と入力すると、Freebase上で利用できる各分野が一覧表示されます。
たとえば絵画や画家が含まれている「Visual Art」の分野を参照して、
(「Visual Artists」という)リストから1番目の画家を取得し、
画家に関する情報を調べることができます：
*)
let art = fb.``Arts and Entertainment``.``Visual Art``

let ldv = art.``Visual Artists``.First()
ldv.``Country of nationality``
ldv.Artworks
(**
このコードを実行してみると、リストの1番目の画家が
レオナルド・ダ・ビンチ(Leonardo da Vinci)であることが確認できるでしょう。
`Country of nationality` プロパティは実際にはlistを返しますが、
今回の場合は単にイタリア(Italy)という1カ国の名前だけが含まれます。
また、 `Artworks` プロパティを使うと画家の作品一覧を取得できます。

次に、日本国籍を持った画家一覧を見つけるクエリを作成します
(簡単のために、国籍リストの最初の1つだけしかチェックしていません)：
*)
// 日本の画家を探すためのクエリ
let artists =
  query { for p in art.``Visual Artists`` do
          where (p.``Country of nationality``.Count() > 0)
          where (p.``Country of nationality``.First().Name = "Japan") }

// 検索された画家全員の名前を表示
for a in artists do 
  printfn "%s (%s)" a.Name a.``Date of birth``
(**
このスニペットを実行すると、Yoshitaka Amano(天野喜孝)やIsamu Noguchi(イサムノグチ)、
Takashi Murakami(村上隆)といった20世紀生まれの画家が最初に表示されます。
`sortBy` や `sortByDescending` を追加して、特定の順序に並び替えることもできます。
また、特定の1人の画家を検索するクエリに `head` を組み合わせて、1人の画家に関する
詳細情報を取得することもできます：
*)
// 北斎に関する詳細情報を検索
let hok = 
  query { for p in art.``Visual Artists`` do
          where (p.Name = "Hokusai")
          head }

// 画像のURLや説明文、作品一覧を表示
printfn "<img src=\"%s\" />" hok.MainImage
printfn "<p>%s</p>" (hok.Blurb.First())
for a in hok.Artworks do
  printfn "<h3>%s</h3><img src=\"%s\" />" a.Name a.MainImage

(**
スニペットの後半では北斎の様々な情報をHTML形式で出力しています。
結果は以下のようになります：

<img src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/447534?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" class="rdecor" />

<blockquote>
<p>
Katsushika Hokusai (葛飾 北斎, September 23, 1760 – May 10, 1849) was a Japanese artist, 
ukiyo-e painter and printmaker of the Edo period. He was influenced by such painters as 
Sesshu, and other styles of Chinese painting. Born in Edo (now Tokyo), Hokusai is best 
known as author of the woodblock print series Thirty-six Views of Mount Fuji (富嶽三十六景, 
Fugaku Sanjūroku-kei, c. 1831) which includes the internationally recognized print, The 
Great Wave off Kanagawa, created during the 1820s. 
</p>
<div class="row">
  <div class="large-3 columns" style="text-align:center">
    <strong>The Dream of the Fisherman's Wife</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/120391?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>The Great Wave off Kanagawa</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/2646210?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Travellers Crossing the Oi River</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h65g7?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Red Fuji</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/313228?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
</div> 
<div class="row">
  <div class="large-3 columns" style="text-align:center">
    <strong>Feminine Wave</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6hz5?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Masculine Wave</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6kpr?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Black Fuji</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6qjh?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Oceans of Wisdom</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/en_id/25174350?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
</div> 
</blockquote>

北斎とフラクタル
----------------

北斎の作品を見てみると、フラクタルのように見えるものがいくつかあることがわかります。
[Hokusai と fractal でGoogle画像検索](https://www.google.com/search?q=hokusai+fractal&tbm=isch)
してみると、北斎の作品とよく似たフラクタルが多数見つかることがわかります。

この記事ではジュリア集合のフラクタルを使います。
このフラクタルは(特にF#を使用すると)非常に簡単に描画できます。
神奈川沖浪裏(The Great Wave off Kanagawa)によく似たフラクタルになるようにするため、
フラクタルを色づけるためのカラーパレットを慎重に選択することになります。

### ジュリア集合の計算

ジュリア集合については [Wikipediaのページ](http://en.wikipedia.org/wiki/Julia_set)
に詳しいので、ここでは動作の詳細については説明しません。
大まかにいえば、複素数のシーケンスを生成して、何らかの定数 <em>c</em> に対して
各ステップ毎に
<em>c<sub>next</sub> = c<sub>prev</sub><sup>2</sup> + c</em>
という式で次の値を計算していきます。
この反復処理は無限のシーケンスを生成する(可能性のある)
再帰的なシーケンス式を書くことでいい具合に作成できます：
*)
open System.Drawing
open System.Numerics

/// それらしいフラクタルを生成するための定数
let c = Complex(-0.70176, -0.3842)

/// 特定の座標に対するシーケンスを生成する
let iterate x y =
  let rec loop current = seq { 
    yield current
    yield! loop (current ** 2.0 + c) }
  loop (Complex(x, y))
(**
この関数は(-1から+1までの)座標を引数にとり、画面上の1ピクセルに対する
シーケンスを生成します。
次に、絶対値が2以上になるまで(あるいは反復回数の最大値が特定の制限値を超えるまで)
反復回数を数える必要があります。
具体的には `Seq` モジュールの関数を以下のように使います：
*)
let countIterations max x y = 
  iterate x y
  |> Seq.take (max - 1)
  |> Seq.takeWhile (fun v -> Complex.Abs(v) < 2.0)
  |> Seq.length
(**
まず、反復回数を特定の値に制限していて、
次に絶対値が小さい値を取得しつづけて、
最後にシーケンスの長さを計算しています。
(絶対値が2.0を超えると `takeWhile` 関数はシーケンスの生成を終了します。
また、 `length` の値は `max - 1` 以下になります。)
このコードは命令的な方法でも実装できます。
実際その方がパフォーマンスがいいかもしれません。
しかし今回の方法の方がジュリア集合の定義に則していることがはっきりわかります。

### カラーパレットの生成

フラクタルを描画するためには、描画したいすべてのピクセルを走査して、
それぞれのピクセルに対して特定の `x` および `y` を引数に指定して
`countInterations` を呼び出し、反復回数に応じた色をパレットから
取得することになります。
北斎の絵画をフラクタルで表現するためには、このパレットを慎重に生成する必要があります。
アイデアとしては神奈川沖浪裏(The Great Wave off Kanagawa)で使われている
多数の色のグラデーションを作成します。

そこで、 `clr1 -- count --> clr2` というような記述ができるように
2つのカスタムF#演算子を用意します。
この式は `clr1` から `clr2` まで、ステップ数 `count` で
グラデーションするような色を表します：
*)
// 2つの色を'count'ステップ数でグラデーションする
let (--) clr count = clr, count
let (-->) ((r1,g1,b1), count) (r2,g2,b2) = [
  for c in 0 .. count - 1 ->
    let k = float c / float count
    let mid v1 v2 = 
      int (float v1 + ((float v2) - (float v1)) * k) 
    Color.FromArgb(mid r1 r2, mid g1 g2, mid b1 b2) ]
(**
`--` 演算子はタプルを生成する構文的なトリックにすぎません。
式は `(clr1 -- count) --> clr2` というように解析されるので、
2番目の演算子は初期色とステップ数を受け取って、
グラデーションを構成する色のリストを生成することができます。

そうすると配列式と `yield!` を使って、各グラデーションを組み合わせることで
うまい具合にパレットを生成することができます。
まず空色から始めて、(水を表す)青系のグラデーションをいくつか生成し、
最後に白系のグラデーションを追加します：
*)
// 北斎の作品で使われているカラーパレット
let palette = 
  [| // 3つの空の色と、ライトブルーまでのグラデーション
     yield! (245,219,184) --3--> (245,219,184) 
     yield! (245,219,184) --4--> (138,173,179)
     // ダークブルーまでのグラデーションと、ミディアムダークブルーまでのグラデーション
     yield! (138,173,179) --4--> (2,12,74)
     yield! (2,12,74)     --4--> (61,102,130)
     // 波の色までのグラデーションと、ライトブルーまで、そして波の色に戻るまでのグラデーション
     yield! (61,102,130)  -- 8--> (249,243,221) 
     yield! (249,243,221) --32--> (138,173,179) 
     yield! (138,173,179) --32--> (61,102,130) |]

(**
### フラクタルの描画

さてこれでフラクタルを描画するためのものがすべて揃いました。
最初のバージョンをシンプルなものにするために、まずはWindowsフォームと
`System.Drawing` の `Bitmap` を使うことにしましょう。
フォームを作成するコードについては省略します
(ただし [GitHub上に完全なソースコード](https://github.com/tpetricek/TomaspNet.Website/tree/master/source/blog/2013) が置いてあります)。

フォームとビットマップを作成した後は、ビットマップの各ピクセルを走査して、
(パレットの長さを最大値として)反復の回数を数えて、
その値をインデックスにしてパレットから色を取得します：
*)

(*[omit:(PictureBoxを持ったフォームを作成)]*)
open System.Windows.Forms

let f = new Form(Visible=true, ClientSize=Size(400, 300))
let i = new PictureBox()
i.SizeMode <- PictureBoxSizeMode.Zoom
i.Dock <- DockStyle.Fill
f.Controls.Add(i)(*[/omit]*)

// 描画する領域を指定する
let w = -0.4, 0.4
let h = -0.95, -0.35

// フォームのサイズに一致するBitmapを作成する
let width = float f.ClientSize.Width
let height = float f.ClientSize.Height
let bmp = new Bitmap(f.ClientSize.Width, f.ClientSize.Height)

// 各ピクセルに対して、指定の領域になるよう変形した後、
// countInterations とパレットを使って色を選択する
for x in 0 .. bmp.Width - 1 do
  for y in 0 .. bmp.Height - 1 do 
    let x' = (float x / width * (snd w - fst w)) + fst w
    let y' = (float y / height * (snd h - fst h)) + fst h
    let it = countIterations palette.Length x' y' 
    bmp.SetPixel(x, y, palette.[it])
i.Image <- bmp
(**
引数 `w` と `h` はそれぞれ描画するフラクタルの部分を表すタプルです。
これらの値を `-2.0, 2.0` と `-1.5, 1.5` に変更すればフラクタル全体が表示されます。
ここでは以下のような素敵な絵が表示されるように、フラクタルの特別な場所を
選択しています：

<div style="text-align:center;padding-right:40px">
<img src="compared.jpg" style="margin-left:auto;margin-right:auto;" />
</div>
*)

(*** hide ***)
#r @"..\packages\FunScript\FunScript.TypeScript.Binding.lib.dll"
let palette = [|(0.0,0.0,0.0)|]
let setPixel (img:ImageData) x y width (r, g, b) = ()
let (?) (doc:Document) name :'R = failwith "!"

(**
Fun(Script)の追加
-----------------

さてこれでフラクタルを描画するコードが出来上がったわけですが、
Windowsフォームプロジェクトを新しく作成することなしに、
ライブで実行される様子を皆さんも見てみたいのではないかと思います。
幸いにも私たちはF#コードをJavaScriptに変換する [FunScript](http://funscript.info/)
というコンパイラが使えるわけなので、HTML5の `<canvas>` 要素に
フラクタルを描画するようにできます。
FunScriptには既にHTML5でマンデルブロ集合を描画するようなサンプルがあるので、
これもまた実に簡単に実装できます。
ただし重要なのは、これまで作成してきたコードを単に再利用することができる、という点です。
詳細をすべて説明するスペースもないので、重要なリンクをいくつか紹介するにとどめます：

 * FunScriptの詳細については [プロジェクトのホームページ](http://funscript.info) を参照してください。
 * [FunScriptバージョンのソースコード](https://github.com/tpetricek/FunScript/tree/master/Examples/Hokusai)
   は私のGitHubにあります。
 * そして、 [実際にコードが動作しているバージョン](hokusai.html) を見ることもできます。

まず描画関数を変更するところから見ていきましょう。
実際にJavaScriptで動作しているバージョンをみてもわかるように、
描画処理がやや遅いため、列を描画する毎に表示している絵を更新しています。
(元々のコードを動的型付けであるJavaScriptへとコピーしているだけなので、
パフォーマンスの低下は想定通りです。
命令的なスタイルに書き直せばコードのパフォーマンスを改善できるでしょう。)

しかし描画関数をF#の `async` ワークフローでラップして、1列描画が終わる度に
`Async.Sleep` を呼んで絵を更新するようにすればかなりいい感じのコードになります：
*)
/// 列毎にスリープしつつ、非同期的にフラクタルを描画する
let render () = async {
  // <canvas> 要素を取得して、描画する画像を作成する
  let canv : HTMLCanvasElement = Globals.document?canvas
  let ctx = canv.getContext_2d()
  let img = ctx.createImageData(float width, float height)
    
  // 各ピクセルに対して、特定の領域になるよう変形した後、
  // countInterations とパレットを使って色を選択する
  for x in 0 .. int width - 1 do
    for y in 0 .. int height - 1 do 
      let x' = (float x / width * (snd w - fst w)) + fst w
      let y' = (float y / height * (snd h - fst h)) + fst h
      let it = countIterations palette.Length x' y' 
      setPixel img x y width palette.[it]

    // ノンブロッキングなスリープを追加＆フラクタルを更新
    do! Async.Sleep(1)
    ctx.putImageData(img, 0.0, 0.0) }
(**
このコードは先ほどのものとほとんど同じです。
なおFunScriptにはJavaScriptライブラリのほとんどの機能に対する型付きのラッパーがあるため、
コード中の変数はいずれも静的に型付けされます(つまり `canv` は `HTMLCanvasElement` 型で、
描画コンテキストを取得する `getContext_2d` メソッドなどを持ちます)。
また、1つのピクセルに色を設定するためのヘルパー関数 `setPixel` や、
IDによってHTML要素を得るための動的検索演算子 `?` を使っています
(いずれも次の節で紹介します)。
描画を開始するにはイベントハンドラをセットアップして `StartImmediate` メソッドを呼び出します：
*)
// 描画を開始するため、ボタンにイベントハンドラを設定する
let go : HTMLButtonElement = Globals.document?go
go.addEventListener_click(fun _ -> 
  render() |> Async.StartImmediate; null)  
(**
ここで使っている2つのヘルパー関数もかなり単純なものです。
`setPixel` は `ImageData` 配列のオフセットを計算してR,G,Bのコンポーネント
(とアルファチャネル)を設定しているだけです。
動的演算子も、 `getElementById` を呼んで、期待される型に要素をキャストして返しているだけです：
*)

/// ImageDataにあるピクセルの値を特定の色に設定する
let setPixel (img:ImageData) x y width (r, g, b) =
  let index = (x + y * int width) * 4
  img.data.[index+0] <- r
  img.data.[index+1] <- g
  img.data.[index+2] <- b
  img.data.[index+3] <- 255.0

/// IDで見つかるHTML要素を返す動的演算子
let (?) (doc:Document) name :'R =  
  doc.getElementById(name) :?> 'R
(**
まとめ
------

まず最初に、私がこの記事を楽しんで書くことが出来たのと同じように、
皆さんが私の記事を楽しく読むことが出来ていますように。
そしてとても素敵なクリスマスシーズンを皆さんが過ごせていますように！

冒頭でも触れましたが、私はここ最近F#コミュニティで開発されている
興味深いライブラリを紹介したいと思っていて、また日本に関連のあるテーマと
絡めて紹介できたらいいなと思っていました。
最初は [F# Data](http://fsharp.github.io/FSharp.Data/) 、
特にFreebase型プロバイダを使って日本の画家とそれぞれの作品のリストを取得しました。

次に北斎を題材にして、適切に選択したパレットを使ったジュリア集合で
彼の作である神奈川沖浪裏の再作成に挑戦しました。
最初のバージョンでは、Windowsフォームを使って描画しました。
Webブラウザ上で直接実行できるコードにするためには、
FunScriptを使ってF#をJavaScriptへと変換しました。
ここでは既存のコードを変更する必要がほとんどありませんでした。
フラクタルの描画進捗を確認できるようにするには、
(ボーナスとして)非同期ワークフローを追加するだけでした。
*)
