// --------------------------------------------------------------------------------------
// DATA AND PREPROCESSING
// --------------------------------------------------------------------------------------

var data =
 [ ["Social protection", 14.10], ["Health", 7.40], ["Education", 4.50],
   ["General public services", 3.10], ["Economic affairs", 2.40] ]
var colors =
  [ "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728", "#9467bd",
    "#8c564b", "#e377c2", "#7f7f7f", "#bcbd22", "#17becf" ]

var nums = data.map(v => v[1])
var sum = nums.reduce((a,b) => a + b)
var max = Math.max.apply(null, nums)
var avg = sum / data.length;
var width = Math.min(600, document.getElementById("out1").offsetWidth);

// --------------------------------------------------------------------------------------
// FINAL COMPLETED CHART
// --------------------------------------------------------------------------------------

(function() {
  var init = {
    animation:0,
    guessed:false,
    max:Math.floor(max * 1.2),
    values:data.map((v, i) => ({
      moved: false,
      color: colors[i],
      category: v[0],
      value: avg,
      correct: v[1],
      random: Math.random()
    })).sort((a, b) => a.random - b.random)
  };

  function update(state, evt) {
    switch(evt.kind) {
      case "animate":
        return {...state, animation:state.animation + 0.02}
      case "set":
        if(state.animation > 0) return state;
        let newValues = state.values.map(v =>
          ({ ...v,
             moved: v.category == evt.category ? true : v.moved,
             value: v.category == evt.category ? evt.value : v.value }))
        return {...state, values:newValues, guessed:newValues.every(v => v.moved) }
    }
    return state;
  }

  function render(trigger, state) {
    if (state.animation > 0 && state.animation < 1)
      window.setTimeout(() => trigger({kind:"animate"}), 10)
    let o = state.guessed ? {} : {disabled:""}
    function handler(x, y, e) {
      if (e.buttons > 0) trigger({kind:"set", value:x, category:y[0] })
    }
    return c.html("div", { class:"youguess" }, [
      c.svg(width, 300, c.axes("bottom", c.on({
          mousemove: handler, mousedown: handler
        },c.overlay(state.values.map(v => {
          let av = v.correct * state.animation + v.value * (1 - state.animation);
          return c.padding(10,0,10,0,c.overlay([
            c.font("13pt sans-serif", v.color, c.text(state.max*0.98, [v.category, 0.5], v.category, "end")),
            c.strokeColor(v.color, c.line([ [v.value, [v.category, 0]], [v.value, [v.category, 1]] ])),
            c.fillColor("#a0a0a030", c.bar(state.max, v.category)),
            c.fillColor(v.color + (v.moved?"90":"30"), c.bar(av, v.category))
          ]));
        }) )))),
      c.html("div", {style:"text-align:center"}, [
        c.html("button", {...o,
          click:() => trigger({kind:"animate"}) }, [ "Show me how I did" ])
      ])
    ]);
  }

  c.interactive("out1", init, update, render)
})();

// --------------------------------------------------------------------------------------
// BASIC BAR CHART
// --------------------------------------------------------------------------------------

(function() {
  var chart =
    c.axes("left bottom",
      c.overlay(data.map((v, i) =>
        c.fillColor(colors[i] + "90", c.bar(v[1], v[0]))
    )))

  c.render("out2", chart)
})();

// --------------------------------------------------------------------------------------
// BASIC BAR CHART
// --------------------------------------------------------------------------------------

(function() {
  var init = {
    animation:0,
    guessed:false,
    max:Math.floor(max * 1.2),
    values:data.map((v, i) => ({
      moved: false,
      color: colors[i],
      category: v[0],
      value: avg,
      correct: v[1],
      random: Math.random()
    })).sort((a, b) => a.random - b.random)
  };

  var state = init

  function drawBar(v, value, fillclr) {
    return c.padding(10,0,10,0,c.overlay([
      c.fillColor("#a0a0a030", c.bar(state.max, v.category)),
      c.fillColor(fillclr, c.bar(value, v.category)),
      c.font("13pt sans-serif", v.color, c.text
        (state.max*0.98, [v.category, 0.5], v.category, "end")),
      c.strokeColor(v.color, c.line([
        [v.value, [v.category, 0]],
        [v.value, [v.category, 1]] ]))
    ]));
  }

  var chart = c.axes("bottom",
    c.overlay(state.values.map(v =>
      drawBar(v, v.correct, v.color+"90") )))
  c.render("out3", chart)
})();


// --------------------------------------------------------------------------------------
// FINAL COMPLETED CHART, TAKE 2
// --------------------------------------------------------------------------------------

(function() {
  var init = {
    animation:0,
    guessed:false,
    max:Math.floor(max * 1.2),
    values:data.map((v, i) => ({
      moved: false,
      color: colors[i],
      category: v[0],
      value: avg,
      correct: v[1],
      random: Math.random()
    })).sort((a, b) => a.random - b.random)
  };

  function update(state, evt) {
    switch(evt.kind) {
      case "animate":
        return {...state, animation:state.animation + 0.02}
      case "set":
        var newValues = state.values.map(v => {
          var match = v.category == evt.category;
          var val = match ? evt.value : v.value;
          return { ...v, moved: match || v.moved, value: val } });
        var newGuessed = newValues.every(v => v.moved);
        return {...state, values:newValues, guessed:newGuessed }
    }
  }


  function render(trigger, state) {
    function drawBar(v, value, fillclr) {
      return c.padding(10,0,10,0,c.overlay([
        c.fillColor("#a0a0a030", c.bar(state.max, v.category)),
        c.fillColor(fillclr, c.bar(value, v.category)),
        c.font("13pt sans-serif", v.color, c.text
          (state.max*0.98, [v.category, 0.5], v.category, "end")),
        c.strokeColor(v.color, c.line([
          [v.value, [v.category, 0]],
          [v.value, [v.category, 1]] ]))
      ]));
    }

    if (state.animation > 0 && state.animation < 1)
      window.setTimeout(() => trigger({kind:"animate"}), 10)
    let o = state.guessed ? {} : {disabled:""}
    function handler(x, y, e) {
      if (state.animation == 0 && e.buttons > 0)
        trigger({kind:"set", value:x, category:y[0] })
    }
    return c.html("div", { class:"youguess" }, [
      c.svg(width, 300, c.axes("bottom", c.on({
          mousemove: handler, mousedown: handler
        },c.overlay(state.values.map(v =>
          drawBar(v, v.correct * state.animation + v.value * (1 - state.animation), v.color + (v.moved?"90":"30"))
        ))))),
      c.html("div", {style:"text-align:center"}, [
        c.html("button", {...o,
          click:() => trigger({kind:"animate"}) }, [ "Show me how I did" ])
      ])
    ]);
  }

  c.interactive("out4", init, update, render)
})();
