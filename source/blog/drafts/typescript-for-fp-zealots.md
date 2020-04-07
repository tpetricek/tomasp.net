TypeScript for F# zealots [DRAFT]
=========================================================

 - title: TypeScript for F# zealots
 - date: 2019-09-08T12:22:57.7521867+01:00
 - description: TBD
 - layout: post
 - references: true
 - image-large: http://tomasp.net/blog/2019/software-engineering/sdi.jpg
 - tags: fsharp

----------------------------------------------------------------------------------------------------

tbd

----------------------------------------------------------------------------------------------------

Classes suck

```
export class ExternalLanguagePlugin implements Langs.LanguagePlugin {
  readonly language: string;
  readonly iconClassName: string;
  readonly editor: Langs.Editor<ExternalState, ExternalEvent>;
  readonly serviceURI: string;
  readonly defaultCode : string;
  readonly regex_global:RegExp = /^%global/;
  readonly regex_local:RegExp = /^%local/;

  constructor(l: string, icon:string, uri: string, code:string) {
    this.language = l;
    this.iconClassName = icon;
    this.serviceURI = uri;
    this.editor = ExternalEditor;
    this.defaultCode = code;
  }
```

But then you will often need classes for safety because you can do `instanceof`
(so you have to move from interfaces to classes!)
---> I use F# style and I have to sacrifice either safety or conciseness (i choose conciseness)


Any ftl

`any` in `axios.get`

Js ftl

```
[{a:"foo",b:"bar"}].join(",")
"[object Object]"
```

FFS

var, const, let

FFS

let completions = (aiaBlock.code.length > 0) ? await getCompletions(aiaBlock.code[aiaBlock.code.length-1].path) : []

does not work
var completions = []
if (aiaBlock.code.length > 0) completions = await getCompletions(aiaBlock.code[aiaBlock.code.length-1].path)
need annotation:
var completions : Completion[] = []

Syntax ffs
(I guess return sucks?)

let createAiaEditor = (assistants:AiAssistant[]) : Langs.Editor<AiaState, AiaEvent> => ({
})

function createAiaEditor(assistants:AiAssistant[]) : Langs.Editor<AiaState, AiaEvent> {
  return ..
}


Nice:

"strictNullChecks": true,
and `T | null`

DUs ftw

```
interface NotebookAddEvent { kind:'add', id: number, language:string }
interface NotebookToggleAddEvent { kind:'toggleadd', id: number }
interface NotebookRemoveEvent { kind:'remove', id: number }
interface NotebookBlockEvent { kind:'block', id:number, event:any }
interface NotebookUpdateTriggerEvalEvent { kind:'evaluate', id:number }
interface NotebookUpdateEvalStateEvent { kind:'evalstate', hash:string, newState:"pending" | "done" }
interface NotebookSourceChange { kind:'rebind', block: Langs.BlockState, newSource: string}

type NotebookEvent =
  NotebookAddEvent | NotebookToggleAddEvent | NotebookRemoveEvent | NotebookUpdateTriggerEvalEvent |
  NotebookBlockEvent | NotebookUpdateEvalStateEvent | NotebookSourceChange
```

Comprehensions FTW

```

var plusBody = [
  h('button', {}, [ h('i', { class:'fa fa-plus'}) ])
];

if (state.expanded)
  plusBody.push(
    h('div', {class:'completion'},
      aiaNode.completions.map(compl =>
        h('a', {class:'item', onclick:() => triggerComplete(compl) }, [ compl.name ])
      )
    ) );

var i = 0;
return h('div',
  { class:'aia',
    onclick:() => ctx.trigger({kind:"expand", expanded:false})
  },
  aiaNode.chain.map(item => {
    var body : any[] = [ item.name ]
    if (++i == aiaNode.chain.length)
      body.push(h('button', { onclick:() => triggerRemove() }, [ h('i', { class:'fa fa-times'}) ]))
    return h('span', {key:'e'+i.toString(), class:'chain'}, body)
  }).concat(
    h('div', {class:'plus', onclick:(e) => {
      e.cancelBubble = true
      ctx.trigger({ kind:"expand", expanded:!state.expanded })
    } }, plusBody)
  ));
```
