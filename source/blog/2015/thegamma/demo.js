// ----------------------- from intellisense.js

var Utils = function () {
  /**
   * Eithers shows the specified element or hides it
   */
  function showElement(el, b) {
    el.style.display = b ? 'block' : 'none';
  }

  /**
   * Check to see if an element has a css class
   */
  function hasCssClass(el, name) {
    var classes = el.className.split(/\s+/g);
    return classes.indexOf(name) !== -1;
  }

  /**
   * Adds a css class from an element
   */
  function addCssClass(el, name) {
    if (!hasCssClass(el, name)) {
      el.className += " " + name;
    }
  }

  /**
   * Removes a css class from an element
   */
  function removeCssClass(el, name) {
    var classes = el.className.split(/\s+/g);
    while (true) {
      var index = classes.indexOf(name);
      if (index == -1) {
        break;
      }
      classes.splice(index, 1);
    }
    el.className = classes.join(" ");
  }

  /**
   * Convenience method to get the last index of any of the items in the array
   */
  function lastIndexOfAny(str, arr, start) {
    var max = -1;
    for (var i = 0; i < arr.length; i++) {
      var val = str.lastIndexOf(arr[i], start);
      max = Math.max(max, val);
    }
    return max;
  }

  function repeat(s, n){
      var a = [];
      while(a.length < n){
          a.push(s);
      }
      return a.join('');
  }

  function escapeIdent(name) {
    if (!isNaN(name[0]) || lastIndexOfAny(name, [' ', '[', ']', '.']) != -1) {
      name = '``' + name + '``';
    }
    return name;
  }


  this.lastIndexOfAny = lastIndexOfAny;
  this.removeCssClass = removeCssClass;
  this.addCssClass = addCssClass;
  this.hasCssClass = hasCssClass;
  this.showElement = showElement;
  this.escapeIdent = escapeIdent;
  this.repeat = repeat;
};


var MethodsIntellisense = function () {
  var utils = new Utils();
  var visible = false;
  var methods = []
  var selectedIndex = 0;

  // methods
  var methodsElement = document.createElement('div');
  methodsElement.className = 'br-methods';

  // methods text
  var methodsTextElement = document.createElement('div');
  methodsTextElement.className = 'br-methods-text';

  // arrows
  var arrowsElement = document.createElement('div');
  arrowsElement.className = 'br-methods-arrows';

  // up arrow
  var upArrowElement = document.createElement('span');
  upArrowElement.className = 'br-methods-arrow';
  upArrowElement.innerHTML = '&#8593;';

  // down arrow
  var downArrowElement = document.createElement('span');
  downArrowElement.className = 'br-methods-arrow';
  downArrowElement.innerHTML = '&#8595;';

  // arrow text (1 of x)
  var arrowTextElement = document.createElement('span');
  arrowTextElement.className = 'br-methods-arrow-text';

  arrowsElement.appendChild(upArrowElement);
  arrowsElement.appendChild(arrowTextElement);
  arrowsElement.appendChild(downArrowElement);
  methodsElement.appendChild(arrowsElement);
  methodsElement.appendChild(methodsTextElement);
  document.body.appendChild(methodsElement);

  /**
   * Sets the selected index of the methods
   */
  function setSelectedIndex(idx) {
    var disabledColor = '#808080';
    var enabledColor = 'black';
    if (idx < 0) {
      idx = methods.length - 1;
    }
    else if (idx >= methods.length) {
      idx = 0;
    }

    selectedIndex = idx;
    methodsTextElement.innerHTML = methods[idx];
    arrowTextElement.innerHTML = (idx + 1) + ' of ' + methods.length;
  }

  /**
   * This method is called by the end-user application to show method information
   */
  function setMethods(data) {
    if (data != null && data.length > 0) {
      methods = data;

      // show the elements
      setVisible(true);

      // show the first item
      setSelectedIndex(0);
    }
  }

  /**
   * Reposition the methods element
   */
  function setPosition(left, top) {
    methodsElement.style.left = left + 'px';
    methodsElement.style.top = top + 'px';
  }

  /**
   * Moves the methods the specified delta
   */
  function moveSelected(delta) {
    setSelectedIndex(selectedIndex + delta);
  }

  /**
   * Checks to see if this is visible
   */
  function isVisible() {
    return visible;
  }

  /**
   * Show the methods UI
   */
  function setVisible(b) {
    visible = b;
    utils.showElement(methodsElement, b);
  }

  // arrow click events
  downArrowElement.onclick = function () {
    moveSelected(1);
  };

  // arrow click events
  upArrowElement.onclick = function () {
    moveSelected(-1);
  };

  this.setVisible = setVisible;
  this.isVisible = isVisible;
  this.setSelectedIndex = setSelectedIndex;
  this.setMethods = setMethods;
  this.moveSelected = moveSelected;
  this.setPosition = setPosition;
};

var DocumentationSide = function()
{
  var utils = new Utils();
  var documentationElement = document.getElementById('editor-documentation-side');

  function showElement(b) {
    utils.showElement(documentationElement, b);
  };

  function showDocumentation(documentation) {
    if (documentation == null || documentation.trim().length == 0) {
      showElement(false);
    }
    else {
      showElement(true);
      if (documentation.trim().indexOf("[JAVASCRIPT]") == 0) {
        eval("(" + documentation.trim().substr("[JAVASCRIPT]".length) + ")")(documentationElement);
      }
      else {
        documentationElement.innerHTML = documentation;
      }
    }
  }

  function moveElement(top) {
    var headerHeight = document.getElementById("header").offsetHeight;
    documentationElement.style.top = (top - headerHeight - 20) + "px";
  }

  this.moveElement = moveElement;
  this.showDocumentation = showDocumentation;
  this.showElement = showElement;
}

var DeclarationsIntellisense = function () {
  var events = { itemChosen: [], itemSelected: [] };
  var utils = new Utils();
  var selectedIndex = 0;
  var filteredDeclarations = [];
  var filteredDeclarationsUI = [];
  var visible = false;
  var declarations = [];
  var documentationSide = new DocumentationSide();

  // ui widgets
  var selectedElement = null;
  var listElement = document.createElement('ul');
  listElement.className = 'br-intellisense';
  document.body.appendChild(listElement);

  /**
   * Filters an array
   */
  function filter(arr, cb) {
    var ret = [];
    arr.forEach(function (item) {
      if (cb(item)) {
        ret.push(item);
      }
    });
    return ret;
  };

  /**
   * Triggers that an item is chosen.
   */
  function triggerItemChosen(item) {
    events.itemChosen.forEach(function (callback) {
      callback(item);
    });
  }

  /**
   * Triggers that an item is selected.
   */
  function triggerItemSelected(item) {
    events.itemSelected.forEach(function (callback) {
      callback(item);
    });
  }

  /**
   * Gets the selected index
   */
  function getSelectedIndex(idx) {
    return selectedIndex;
  }

  /**
   * Sets the selected index
   */
  function setSelectedIndex(idx) {
    if (idx != selectedIndex) {
      selectedIndex = idx;
      triggerItemSelected(getSelectedItem());
    }
  }

  /**
   * Event when an item is chosen (double clicked).
   */
  function onItemChosen(callback) {
    events.itemChosen.push(callback);
  }

  /**
   * Event when an item is selected.
   */
  function onItemSelected(callback) {
    events.itemSelected.push(callback);
  }

  /**
   * Gets the selected item
   */
  function getSelectedItem() {
    return filteredDeclarations[selectedIndex];
  }

  /**
   * Creates a list item that is appended to our intellisense list
   */
  function createListItemDefault(item, idx) {
    var listItem = document.createElement('li');
    listItem.innerHTML = '<span class="br-icon icon-glyph-' + item.glyph + '"></span> ' + item.name;
    listItem.className = 'br-listlink'
    return listItem;
  }

  /**
   * Refreshes the user interface for the selected element
   */
  function refreshSelected() {
    if (selectedElement != null) {
      utils.removeCssClass(selectedElement, 'br-selected');
    }

    selectedElement = filteredDeclarationsUI[selectedIndex];
    if (selectedElement) {
      utils.addCssClass(selectedElement, 'br-selected');

      var item = getSelectedItem();
      documentationSide.showDocumentation(item.documentation);

      var top = selectedElement.offsetTop;
      var bottom = top + selectedElement.offsetHeight;
      var scrollTop = listElement.scrollTop;
      if (top <= scrollTop) {
        listElement.scrollTop = top;
      }
      else if (bottom >= scrollTop + listElement.offsetHeight) {
        listElement.scrollTop = bottom - listElement.offsetHeight;
      }
    }
  }

  /**
   * Refreshes the user interface.
   */
  function refreshUI() {
    listElement.innerHTML = '';
    filteredDeclarationsUI = [];
    filteredDeclarations.forEach(function (item, idx) {
      var listItem = createListItemDefault(item, idx);

      listItem.ondblclick = function () {
        setSelectedIndex(idx);
        triggerItemChosen(getSelectedItem());
        setVisible(false);
        documentationSide.showElement(false);
      };

      listItem.onclick = function () {
        setSelectedIndex(idx);
      };

      listElement.appendChild(listItem);
      filteredDeclarationsUI.push(listItem);
    });

    refreshSelected();
  }

  /**
   * Show the auto complete and the documentation elements
   */
  function setVisible(b) {
    visible = b;
    utils.showElement(listElement, b);
    documentationSide.showElement(b);
  }

  /**
   * This method is called by the end-user application
   */
  function setDeclarations(data) {
    if (data != null && data.length > 0) {
      // set the data
      declarations = data;
      filteredDeclarations = data;

      // show the elements
      setVisible(true);
      documentationSide.showElement(true);
      setFilter('');
    }
  }

  /**
   * Sets the position of the list element and documentation element
   */
  function setPosition(left, top) {
    // reposition intellisense
    listElement.style.left = left + 'px';
    listElement.style.top = top + 'px';

    // reposition documentation
    documentationSide.moveElement(top);
  }

  /**
   * Refresh the filter
   */
  function setFilter(filterText) {
    if (filterText.indexOf("``") == 0)
      filterText = filterText.substr(2);

    // Only apply the filter if there is something left, otherwise leave unchanged
    var filteredTemp = filter(declarations, function (x) {
      return x.name.toLowerCase().indexOf(filterText) === 0;
    });
    if (filteredTemp.length > 0)
      filteredDeclarations = filteredTemp;

    selectedIndex = 0;
    refreshUI();
  }

  /**
   * Moves the auto complete selection up or down a specified amount
   */
  function moveSelected(delta) {
    var idx = selectedIndex + delta;
    idx = Math.max(idx, 0);
    idx = Math.min(idx, filteredDeclarations.length - 1);

    // select
    setSelectedIndex(idx)
    refreshSelected();
  }

  /**
   * Is the list visible or not
   */
  function isVisible() {
    return visible;
  }

  // public API
  this.isVisible = isVisible;
  this.setFilter = setFilter;
  this.getSelectedItem = getSelectedItem;
  this.getSelectedIndex = getSelectedIndex;
  this.setSelectedIndex = setSelectedIndex;
  this.onItemChosen = onItemChosen;
  this.onItemSelected = onItemSelected;
  this.moveSelected = moveSelected;
  this.setDeclarations = setDeclarations;
  this.setPosition = setPosition;
  this.setVisible = setVisible;
};

// ----------------------- from custom.js


function startSpinning(el)
{
  var counter = 0;
  var finished = false;
  function spin() {
    if (finished) {
      el.style.display = "none";
      return;
    } else {
      el.style.display = "block";
      var offset = counter * -21;
      el.style.backgroundPosition = "0px " + offset + "px";
      counter++; if (counter >= 19) counter = 0;
      setTimeout(spin, 100);
    }
  };
  setTimeout(spin, 500);
  return function () { finished = true; };
}

/**************************************** Common for editors/visualizers ****************************************/

function setSource(id, source, byHand)
{
  window[id + "_source"] = source;
  window[id + "_change"].forEach(function (f) { f(byHand); });
}

/**************************************** Setting up the editor ****************************************/

function refreshOutput(id)
{
  var source = window[id + "_source"];
  $.ajax({
    url: "http://thegamma.net/run", data: source, contentType: "text/fsharp",
    type: "POST", dataType: "text"
  }).done(function (data) {
    //finished = true;
    eval("(function(){ \n\
      var outputElementID = \"" + id + "\"\n\
      var blockCallback = function () {}; \n" +
      data + "\n\
    })()");
  });
}

function setupEditor(id) {
  var source = window[id + "_source"];
  var element = document.getElementById(id + "_editor");

  // Setup the CodeMirror editor with fsharp mode
  var editor = CodeMirror(element, {
    value: source, mode: 'fsharp', lineNumbers: false
  });
  editor.focus();

  // Update text when it is changed by visualizers
  var updating = false;
  editor.on("change", function () {
    if (updating) return;
    updating = true;
    setSource(id, editor.getValue(),true);
    updating = false;
  });
  window[id + "_change"].push(function() {
    if (updating) return;
    updating = true;
    var source = window[id + "_source"];
    editor.getDoc().setValue(source);
    updating = false;
  });


  // Helper to send request to our server
  function request(operation, line, col) {
    var url = "http://thegamma.net/" + operation;
    if (line != null && col != null) url += "?line=" + (line + 1) + "&col=" + col;
    return $.ajax({
      url: url, data: editor.getValue(),
      contentType: "text/fsharp", type: "POST", dataType: "JSON"
    });
  }

  // Translate code to JS and then evaluate the script
  /*
  var evaluateScript = function () {
    var counter = 0;
    var finished = false;
    function spin() {
      if (finished) {
        document.getElementById("spinner").style.display = "none";
        return;
      } else {
        document.getElementById("spinner").style.display = "block";
        var offset = counter * -21;
        document.getElementById("spinner").style.backgroundPosition = "0px " + offset + "px";
        counter++; if (counter >= 19) counter = 0;
        setTimeout(spin, 100);
      }
    };
    setTimeout(spin,500);

    $.ajax({
      url: "/run", data: editor.getValue(), contentType: "text/fsharp",
      type: "POST", dataType: "text"
    }).done(function (data) {
      finished = true;
      eval("(function(){ " + data + "})()");
    });
  };*/

  // Request type-checking and parsing errors from the server
  editor.compiler = new Compiler(editor, function () {
    request("check").done(function (data) {
      editor.compiler.updateMarkers(data.errors);
    });
  });

  // Request declarations & method overloads from the server
  editor.intellisense = new Intellisense(editor,
    function (position) {
      request("declarations", position.lineIndex, position.columnIndex).done(function (data) {
        editor.intellisense.setDeclarations(data.declarations);
      });
    },
    function (position) {
      request("methods", position.lineIndex, position.columnIndex).done(function (data) {
        editor.intellisense.setMethods(data.methods);
      });
    }
  );
  $(element).addClass("editor-cm-visible");
  return editor;
}

function switchEditor(id)
{
  var edEl = $("#" + id + "_editor");
  if (edEl.is(":hidden")) {
    edEl.show();
    if (window[id + "_cm_editor"] == null)
      window[id + "_cm_editor"] = setupEditor(id);
  }
  else {
    edEl.hide();
  }
}

/**************************************** Setting up the editor ****************************************/

function setupDocEditor(id) {
  var source = window[id + "_source"];
  var element = document.getElementById(id + "_editor");
  var editor = CodeMirror(element, {
    value: source, mode: 'markdown', lineNumbers: false
  });
  editor.focus();
  editor.on("change", function () {
    window[id + "_source"] = editor.getValue();
  });
  $(element).addClass("editor-cm-visible");
  return editor;
}

function setupRefresh(id)
{
  var source1 = window[id + "_source"];
  var lastRendered = source1;
  var intervalId = setInterval(function () {
    var source2 = window[id + "_source"];
    if (source1 == source2) {
      if (lastRendered != source2) {
        lastRendered = source2;
        $.ajax({
          url: "http://thegamma.net/markdown", data: source2, contentType: "text/markdown",
          type: "POST", dataType: "text"
        }).done(function (data) {
          document.getElementById(id + "_output_html").innerHTML = data;
        });
      }
    }
    source1 = source2;
  }, 1000);
  return intervalId;
}

function switchDocEditor(id) {
  var wrEl = $("#" + id + "_doc_wrapper");
  var tlEl = $("#" + id + "_tools");
  if (wrEl.is(":hidden")) {
    wrEl.show();
    tlEl.hide();

    var intervalId = setupRefresh(id);
    window[id + "_cleanup"] = function () { clearInterval(intervalId); };

    if (window[id + "_cm_editor"] == null)
      window[id + "_cm_editor"] = setupDocEditor(id);
  }
  else {
    window[id + "_cleanup"]();
    tlEl.show();
    wrEl.hide();
  }
}

/**************************************** Setting up the visualizers ****************************************/

function createVisualizer(id, multiple, vis) {
  var source = window[id + "_source"];
  var utils = new Utils();
  var documentationSide = new DocumentationSide();

  // Create chosen slect element and add options from the visualizer
  var sel = $(multiple?'<select multiple />':'<select />');
  vis.options.forEach(function (v) {
    $('<option />').text(v.member).val(v.member).appendTo(sel);
  });
  sel.val(vis.initial);
  $("#" + id + "_visual").append(sel);
  sel.chosen();

  // Code to update the source code when the selection is changed
  if (multiple)
  {
    var range = { startl: vis.range[0], startc: vis.range[1], endl: vis.range[2], endc: vis.range[3] };
    var prefix = vis.prefix.join(".");
    var origLength = range.endl - range.startl + 1;

    function updateSource1() {
      var lines = window[id + "_source"].split('\n');
      var names = sel.val();
      var newLines = [];
      var newEndc, newEndl;

      window[id + "_offsetf"] = function(l) {
        if (l > range.startl) return l + names.length - origLength;
        else return l;
      };

      for(var i = 0; i < lines.length-(range.endl-range.startl+1)+names.length; i++)
      {
        var line;
        var ni = i-range.startl+1;
        if (i == range.startl-1)
          line = lines[i].slice(0, range.startc) + prefix + "." + utils.escapeIdent(names[ni]);
        else if (i > range.startl-1 && ni < names.length-1)
          line = utils.repeat(' ', range.startc) + prefix + "." + utils.escapeIdent(names[ni]);
        else if (ni == names.length-1)
        {
          var suffix = lines[range.endl-1].slice(range.endc+1);
          line = utils.repeat(' ', range.startc) + prefix + "." + utils.escapeIdent(names[ni]) + suffix;
          newEndl = i + 1;
          newEndc = line.length - suffix.length - 1;
        }
        else if (ni > names.length-1)
          line = lines[i-names.length+(range.endl-range.startl+1)];
        else
          line = lines[i];
        newLines.push(line);
      }
      range.endl = newEndl;
      range.endc = newEndc;
      setSource(id, newLines.join("\n"),false);
    };
    sel.on("change", updateSource1);
  }
  else
  {
    var range = { line: vis.range[0], start: vis.range[1], end: vis.range[3] };
    function updateSource2() {
      var f = window[id + "_offsetf"];
      if (!f) f = function(l) { return l; };

      var lines = window[id + "_source"].split('\n');
      var line = lines[f(range.line) - 1];
      var name = utils.escapeIdent(sel.val());
      lines[f(range.line) - 1] = line.slice(0, range.start) + name + line.slice(range.end + 1);
      range.end = range.start + name.length - 1;
      setSource(id, lines.join("\n"),false);
    };
    sel.on("change", updateSource2);
  }

  // Update the documentation side bar when something happens
  function updateDocumentation(member) {
    vis.options.forEach(function (v) {
      if (v.member == member) {
        documentationSide.showDocumentation(v.documentation);
        documentationSide.moveElement(sel.parent().offset().top);
      }
    });
  }
  sel.on("chosen:hiding_dropdown", function() {
    documentationSide.showElement(false);
  });
  sel.on("chosen:showing_dropdown", function() {
    function getCurrent() {
      updateDocumentation($(".chosen-results .highlighted").text());
    }
    $(".chosen-search").on("keyup", getCurrent); // for single-choice
    $(".chosen-choices").on("keyup", getCurrent); // for multi-choice
    $(".chosen-results").on("mouseover", getCurrent);
  });
}

function setupVisualizer(id) {
  var source = window[id + "_source"];
  $.ajax({
    url: "http://thegamma.net/visualizers", data: source,
    contentType: "text/fsharp", type: "POST", dataType: "JSON"
  }).done(function (data) {
    if (data.hash == window[id + "_vis_hash"]) return;

    window[id + "_vis_hash"] = data.hash;
    $("#" + id + "_visual").empty();
    data.singleLevel.forEach(function (vis) { createVisualizer(id, false, vis); });
    data.list.forEach(function (vis) { createVisualizer(id, true, vis); });
  });
}

function switchVisualizer(id) {
  var visEl = $("#" + id + "_visual");
  if (visEl.is(":hidden")) {
    visEl.show();
    if (window[id + "_vis_created"] != true)
    {
      setupVisualizer(id);
      window[id + "_vis_created"]=true;
      window[id + "_change"].push(function (byHand) {
        if (!byHand) return;
        var source1 = window[id + "_source"];
        setTimeout(function () {
          var source2 = window[id + "_source"];
          if (source1 == source2)
            setupVisualizer(id);
        }, 5000)
      });
    }
  }
  else {
    visEl.hide();
  }
}
/**************************************** TheGamma.Data ****************************************/

function showCountryDocumentation(el, info)
{
    while (el.hasChildNodes()) {
        el.removeChild(el.lastChild);
    }

    var list = "";
    function append(k, v) {
        if (v != null && v != "") {
            v = v.replace("(all income levels)", ""); // drop unnecessary noise
            list += "<dt>" + k + "</dt><dd>" + v + "</dd>";
        }
    };
    append("Capital city", info.capital);
    append("Populaion", info.population);
    append("Income level", info.income);
    append("Region", info.region);

    el.innerHTML = "<h2>" + info.name + "</h2><dl>" + list + "</dl>";

    var map = document.createElement("div");
    map.className = "map";
    el.appendChild(map);
    var data = google.visualization.arrayToDataTable([
        ['Country', 'Value'], [info.name, 100]
    ]);
    var options = {
        tooltip: { trigger: 'none' },
        region: info.regionCode,
        colorAxis: { colors: ['white', '#404040'] },
        backgroundColor: '#f8f8f8',
        datalessRegionColor: '#e0e0e0',
        defaultColor: '#e0e0e0',
        legend: 'none'
    };
    var chart = new google.visualization.GeoChart(map);
    chart.draw(data, options);
}

var chartsToDraw = [];
var googleLoaded = false;
function drawChartOnLoad(f) {
  if (googleLoaded) f();
  else chartsToDraw.push(f);
};
google.load('visualization', '1', { 'packages': ['corechart'] });
google.setOnLoadCallback(function () {
  googleLoaded = true;
  for (var i = 0; i < chartsToDraw.length; i++) chartsToDraw[i]();
  chartsToDraw = undefined;
});

function drawChart(chart, data, id, callback) {
  drawChartOnLoad(function() {
    var ctor = eval("(function(a) { return new google.visualization." + chart.typeName + " (a); })");
    var ch = ctor(document.getElementById(id));
    if (chart.options.height == undefined)
      chart.options.height = 400;
    ch.draw(data, chart.options);
    callback();
  });
}


// ----------------------- from the generated HTML

google.load('visualization', '1', { 'packages': ['corechart'] });
google.setOnLoadCallback(function () {

//(function () {
  var stop = startSpinning(document.getElementById("output_2_spinner"));
  var outputElementID = "output_2";
  var blockCallback = function () {
    stop();
    $('#output_2_wrapper').addClass("loaded");
  };
  var series__create$String__Double_String_Double, series_2_String__Object_____ctor$String_Object___, series_2_String__Double__set$String__Object___String_Double_String_Object___, series_2_String__Double___ctor$String_Double, list_1_Tuple_2_String__String__NilTuple_2_String__String_, list_1_Tuple_2_String__String__ConsTuple_2_String__String_, list_1_String__NilString, list_1_String__ConsString, list_1_FSharpAsync_1_Object____NilFSharpAsync_1_Object___, list_1_FSharpAsync_1_Object____ConsFSharpAsync_1_Object___, chart__geo$, Year___ctor$, WorldBank__worldBankUrl$, WorldBank__worldBankDownloadAll$, WorldBank__worldBankDownload$, WorldBank__getYear$, WorldBank__getCountryIds$, WorldBank__getByYear$, UnfoldEnumerator_2_Int32__String___ctor$Int32_String, UnfoldEnumerator_2_Int32__Object_____ctor$Int32_Object___, UnfoldEnumerator_2_Int32__Int32___ctor$Int32_Int32, UnfoldEnumerator_2_IEnumerator_1_Int32__FSharpAsync_1_Object_____ctor$IEnumerator_1_Int32__FSharpAsync_1_Object___, UnfoldEnumerator_2_FSharpList_1_String__String___ctor$FSharpList_1_String__String, UnfoldEnumerator_2_FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object_____ctor$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___, TupleString_String, TupleString_Object___, TupleString_Int32, TupleString_FSharpList_1_String_, TupleString_Double, TupleSetTree_1_String__SetTree_1_String_, TupleObject____Int32, TupleInt32_Int32, TupleFSharpFunc_2_String__Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_, TupleFSharpFunc_2_Object______Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_, TupleFSharpAsync_1_Object____IEnumerator_1_Int32_, TupleFSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___, Set__OfSeq$String_String, Set_1_String___ctor$String, Set_1_String__From$String, Set_1_IComparable__get_Tree$IComparable_, Set_1_IComparable__get_Comparer$IComparable_, Set_1_IComparable__Contains$IComparable_, SetTree_1_String__SetOneString, SetTree_1_String__SetNodeString, SetTree_1_String__SetEmptyString, SetTreeModule__tolerance, SetTreeModule__rebalance$String_String, SetTreeModule__ofSeq$String_String, SetTreeModule__mkFromEnumerator$String_String, SetTreeModule__mk$String_String, SetTreeModule__mem$IComparable_IComparable_, SetTreeModule__height$String_String, SetTreeModule__get_tolerance$, SetTreeModule__add$String_String, SetTreeModule__SetOne$String_String, SetTreeModule__SetNode$String_String, Seq__Unfold$Int32__String_Int32_String, Seq__Unfold$Int32__Object___Int32_Object___, Seq__Unfold$Int32__Int32_Int32_Int32, Seq__Unfold$IEnumerator_1_Int32__FSharpAsync_1_Object___IEnumerator_1_Int32__FSharpAsync_1_Object___, Seq__Unfold$FSharpList_1_String__String_FSharpList_1_String__String, Seq__Unfold$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___, Seq__ToList$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__ToArray$String_String, Seq__ToArray$Object___Object___, Seq__ToArray$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__ToArray$Double_Double, Seq__OfList$String_String, Seq__OfList$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__OfArray$String_String, Seq__OfArray$Object___Object___, Seq__Map$Int32__FSharpAsync_1_Object___Int32_FSharpAsync_1_Object___, Seq__IterateIndexed$String_String, Seq__IterateIndexed$Object___Object___, Seq__IterateIndexed$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__IterateIndexed$Double_Double, Seq__FromFactory$String_String, Seq__FromFactory$Object___Object___, Seq__FromFactory$Int32_Int32, Seq__FromFactory$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__FoldIndexedAux$Unit__String_Unit__String, Seq__FoldIndexedAux$Unit__Object___Unit__Object___, Seq__FoldIndexedAux$Unit__FSharpAsync_1_Object___Unit__FSharpAsync_1_Object___, Seq__FoldIndexedAux$Unit__Double_Unit__Double, Seq__FoldIndexedAux$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___, Seq__FoldIndexed$String__Unit_String_Unit_, Seq__FoldIndexed$Object____Unit_Object____Unit_, Seq__FoldIndexed$FSharpAsync_1_Object____Unit_FSharpAsync_1_Object____Unit_, Seq__FoldIndexed$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___, Seq__FoldIndexed$Double__Unit_Double_Unit_, Seq__Fold$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___, Seq__Enumerator$String_String, Seq__Enumerator$Object___Object___, Seq__Enumerator$Int32_Int32, Seq__Enumerator$FSharpAsync_1_Object___FSharpAsync_1_Object___, Seq__Enumerator$Double_Double, Seq__Delay$FSharpAsync_1_Object___FSharpAsync_1_Object___, Range__oneStep$Int32_Int32, Range__customStep$Int32__Int32_Int32_Int32, Option__Map$IEnumerable_1_String__String___IEnumerable_1_String__String___, Option__Map$IEnumerable_1_Double__Double___IEnumerable_1_Double__Double___, Option__IsSome$Int32_Int32, Option__IsSome$IEnumerator_1_Int32_IEnumerator_1_Int32_, Option__IsSome$FSharpList_1_String_FSharpList_1_String_, Option__IsSome$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___, Option__IsSome$Decimal_Decimal, Option__IsNone$Decimal_Decimal, Option__GetValue$Tuple_2_String__Int32_Tuple_2_String__Int32_, Option__GetValue$Tuple_2_String__FSharpList_1_String_Tuple_2_String__FSharpList_1_String_, Option__GetValue$Tuple_2_String__Double_Tuple_2_String__Double_, Option__GetValue$Tuple_2_Object____Int32_Tuple_2_Object____Int32_, Option__GetValue$Tuple_2_Int32__Int32_Tuple_2_Int32__Int32_, Option__GetValue$Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_, Option__GetValue$Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___, Option__GetValue$String___String___, Option__GetValue$String_String, Option__GetValue$Object___Object___, Option__GetValue$Object_Object_, Option__GetValue$Int32_Int32, Option__GetValue$IEnumerator_1_Int32_IEnumerator_1_Int32_, Option__GetValue$IEnumerable_1_String_IEnumerable_1_String_, Option__GetValue$IEnumerable_1_Double_IEnumerable_1_Double_, Option__GetValue$FSharpRef_1_Boolean_FSharpRef_1_Boolean_, Option__GetValue$FSharpList_1_String_FSharpList_1_String_, Option__GetValue$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___, Option__GetValue$Double___Double___, Option__GetValue$Double_Double, Option__GetValue$Decimal_Decimal, Option__GetValue$CancellationToken_CancellationToken_, Operations__series_2_mapPairs$String__Double__Object___String_Double_Object___, Operations__lift$String__Double__String__Object___String_Double_String_Object___, List__Tail$String_String, List__Tail$FSharpAsync_1_Object___FSharpAsync_1_Object___, List__Reverse$String_String, List__Reverse$FSharpAsync_1_Object___FSharpAsync_1_Object___, List__Map$Tuple_2_String__String__String_Tuple_2_String__String__String, List__Map$String__String_String_String, List__Head$String_String, List__Head$FSharpAsync_1_Object___FSharpAsync_1_Object___, List__FoldIndexedAux$list_1_String__Tuple_2_String__String_list_1_String__Tuple_2_String__String_, List__FoldIndexedAux$list_1_String__String_list_1_String__String, List__FoldIndexedAux$list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___, List__FoldIndexed$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_, List__FoldIndexed$String__list_1_String_String_list_1_String_, List__FoldIndexed$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___, List__Fold$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_, List__Fold$String__list_1_String_String_list_1_String_, List__Fold$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___, List__Empty$Tuple_2_String__String_Tuple_2_String__String_, List__Empty$String_String, List__Empty$FSharpAsync_1_Object___FSharpAsync_1_Object___, List__CreateCons$Tuple_2_String__String_Tuple_2_String__String_, List__CreateCons$String_String, List__CreateCons$FSharpAsync_1_Object___FSharpAsync_1_Object___, LanguagePrimitives__UnboxGeneric$IDisposable_IDisposable_, Json__getArrayMembersByTag$, Json__getArrayMemberByTag$, Helpers__tryGetGlobal$Object_Object_, Helpers__setGlobal$FSharpSet_1_String_FSharpSet_1_String_, Helpers__jsTypeOf$, Helpers__isNull$, Helpers__isArray$, Helpers__getProperty$Object_Object_, Helpers__getJSONPrefix$, Helpers__getGlobal$Object_Object_, Helpers__getGlobal$FSharpSet_1_String_FSharpSet_1_String_, Helpers__encodeURIComponent$, Helpers_1_showChart$Geo_Geo_, Helpers_1_right$ChartColorAxis__String___ChartColorAxis__String___, Helpers_1_right$ChartColorAxis__Double___ChartColorAxis__Double___, Helpers_1_right$ChartColorAxis__Double_ChartColorAxis__Double, Helpers_1_drawChart$Object_Object_, Helpers_1_copy$ChartColorAxis__ChartLegend_ChartColorAxis__ChartLegend_, Geo__show$, Geo__colorAxis$, Geo___ctor$, GeoChartOptions___ctor$, GenericConstants__Zero$Int32_Int32, GenericConstants__One$Int32_Int32, GenericComparer_1_String___ctor$String, FSharpString__Concat$, Extensions__GeoChartOptions_get_empty_Static$, DateTime__createUnsafe$, DateTime__Parse$, CreateEnumerable_1_String___ctor$String, CreateEnumerable_1_Object_____ctor$Object___, CreateEnumerable_1_Int32___ctor$Int32, CreateEnumerable_1_FSharpAsync_1_Object_____ctor$FSharpAsync_1_Object___, ChartData___ctor$, ChartDataModule__oneKeyValue$String_String, ChartColorAxis___ctor$, CancellationToken___ctor$, CancellationToken__ThrowIfCancellationRequested$, Async__StartImmediate$, Async__FromContinuations$String_String, Async__FromContinuations$Object_____Object_____, Async_2_PseudoParallel$Object___Object___, Async_1_protectedCont$Unit_Unit_, Async_1_protectedCont$Tuple_2___Tuple_2___, Async_1_protectedCont$Tuple_2___1Tuple_2___1, Async_1_protectedCont$String_String, Async_1_protectedCont$Object_____Object_____, Async_1_protectedCont$Object___Object___, Async_1_protectedCont$FSharpSet_1_String_FSharpSet_1_String_, Async_1_protectedCont$DataTable_DataTable_, Async_1_invokeCont$Unit_Unit_, Async_1_invokeCont$Tuple_2___Tuple_2___, Async_1_invokeCont$Tuple_2___1Tuple_2___1, Async_1_invokeCont$Object___Object___, Async_1_invokeCont$FSharpSet_1_String_FSharpSet_1_String_, Async_1_invokeCont$DataTable_DataTable_, Async_1_get_async$, Async_1_Unit__ContUnit_, Async_1_Tuple_2____ContTuple_2___, Async_1_Tuple_2___1_ContTuple_2___1, Async_1_String__ContString, Async_1_Object______ContObject_____, Async_1_Object____ContObject___, Async_1_FSharpSet_1_String__ContFSharpSet_1_String_, Async_1_DataTable__ContDataTable_, AsyncParams_1_Unit___ctor$Unit_, AsyncParams_1_Tuple_2_____ctor$Tuple_2___, AsyncParams_1_Tuple_2___1__ctor$Tuple_2___1, AsyncParams_1_String___ctor$String, AsyncParams_1_Object_____ctor$Object___, AsyncParams_1_Object_______ctor$Object_____, AsyncParams_1_FSharpSet_1_String___ctor$FSharpSet_1_String_, AsyncParams_1_DataTable___ctor$DataTable_, AsyncParamsAux___ctor$, AsyncBuilder___ctor$, AsyncBuilder__Zero$, AsyncBuilder__TryWith$Unit_Unit_, AsyncBuilder__Return$Tuple_2___Tuple_2___, AsyncBuilder__Return$Tuple_2___1Tuple_2___1, AsyncBuilder__Return$Object___Object___, AsyncBuilder__Return$FSharpSet_1_String_FSharpSet_1_String_, AsyncBuilder__Return$DataTable_DataTable_, AsyncBuilder__Delay$Unit_Unit_, AsyncBuilder__Delay$Tuple_2___Tuple_2___, AsyncBuilder__Delay$Tuple_2___1Tuple_2___1, AsyncBuilder__Delay$Object___Object___, AsyncBuilder__Delay$FSharpSet_1_String_FSharpSet_1_String_, AsyncBuilder__Delay$DataTable_DataTable_, AsyncBuilder__Bind$Tuple_2____Tuple_2___1Tuple_2____Tuple_2___1, AsyncBuilder__Bind$Tuple_2___1_DataTable_Tuple_2___1_DataTable_, AsyncBuilder__Bind$String__Object___String_Object___, AsyncBuilder__Bind$String__FSharpSet_1_String_String_FSharpSet_1_String_, AsyncBuilder__Bind$Object______Object___Object______Object___, AsyncBuilder__Bind$Object____Unit_Object____Unit_, AsyncBuilder__Bind$Object____Tuple_2___Object____Tuple_2___, AsyncBuilder__Bind$FSharpSet_1_String__Tuple_2___FSharpSet_1_String__Tuple_2___, AsyncBuilder__Bind$DataTable__Unit_DataTable__Unit_, Array__ZeroCreate$Tuple_2_String__Object___Tuple_2_String__Object___, Array__ZeroCreate$Tuple_2_String__Double_Tuple_2_String__Double_, Array__ZeroCreate$String_String, Array__ZeroCreate$Object___Object___, Array__ZeroCreate$Object_Object_, Array__ZeroCreate$FSharpOption_1_Object___FSharpOption_1_Object___, Array__ZeroCreate$FSharpAsync_1_Object___FSharpAsync_1_Object___, Array__ZeroCreate$Double_Double, Array__MapIndexed$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___, Array__MapIndexed$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___, Array__MapIndexed$Object__String_Object__String, Array__MapIndexed$Object__Object_Object__Object_, Array__MapIndexed$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___, Array__MapIndexed$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___, Array__Map$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___, Array__Map$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___, Array__Map$Object__String_Object__String, Array__Map$Object__Object_Object__Object_, Array__Map$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___, Array__Map$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___, Array__Length$Tuple_2_String__Object___Tuple_2_String__Object___, Array__Length$Tuple_2_String__Double_Tuple_2_String__Double_, Array__Length$Object_Object_, Array__Length$FSharpOption_1_Object___FSharpOption_1_Object___, Array__Length$FSharpAsync_1_Object___FSharpAsync_1_Object___, Array__Filter$Object_Object_, Array__ConcatImpl$Object_Object_, Array__Concat$Object_Object_, Array__Choose$Object__Tuple_2_String__Double_Object__Tuple_2_String__Double_, Array__Choose$Object__Object_Object__Object_, Array__BoxedLength$, Array__Append$Object___Object___;
Array__Append$Object___Object___ = (function(xs,ys)
{
  return xs.concat(ys);;
});
Array__BoxedLength$ = (function(xs)
{
  return xs.length;;
});
Array__Choose$Object__Object_Object__Object_ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Object_Object_(0);
  var j = 0;
  for (var i = 0; i <= (Array__Length$Object_Object_(xs) - 1); i++)
  {
    var matchValue = f(xs[i]);
    if ((matchValue.Tag == 0.000000)) 
    {
      ;
    }
    else
    {
      var y = Option__GetValue$Object_Object_(matchValue);
      ys[j] = y;
      null;
      j = (j + 1);
      null;
    };
  };
  return ys;
});
Array__Choose$Object__Tuple_2_String__Double_Object__Tuple_2_String__Double_ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Tuple_2_String__Double_Tuple_2_String__Double_(0);
  var j = 0;
  for (var i = 0; i <= (Array__Length$Object_Object_(xs) - 1); i++)
  {
    var matchValue = f(xs[i]);
    if ((matchValue.Tag == 0.000000)) 
    {
      ;
    }
    else
    {
      var y = Option__GetValue$Tuple_2_String__Double_Tuple_2_String__Double_(matchValue);
      ys[j] = y;
      null;
      j = (j + 1);
      null;
    };
  };
  return ys;
});
Array__Concat$Object_Object_ = (function(xs)
{
  return (function(xss)
  {
    return Array__ConcatImpl$Object_Object_(xss);
  })((function(source)
  {
    return Seq__ToArray$Object___Object___(source);
  })(xs));
});
Array__ConcatImpl$Object_Object_ = (function(xss)
{
  return [].concat.apply([], xss);;
});
Array__Filter$Object_Object_ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Object_Object_(0);
  var j = 0;
  for (var i = 0; i <= (Array__Length$Object_Object_(xs) - 1); i++)
  {
    if (f(xs[i])) 
    {
      ys[j] = xs[i];
      null;
      j = (j + 1);
      null;
    }
    else
    {
      ;
    };
  };
  return ys;
});
Array__Length$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  return xs.length;;
});
Array__Length$FSharpOption_1_Object___FSharpOption_1_Object___ = (function(xs)
{
  return xs.length;;
});
Array__Length$Object_Object_ = (function(xs)
{
  return xs.length;;
});
Array__Length$Tuple_2_String__Double_Tuple_2_String__Double_ = (function(xs)
{
  return xs.length;;
});
Array__Length$Tuple_2_String__Object___Tuple_2_String__Object___ = (function(xs)
{
  return xs.length;;
});
Array__Map$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___ = (function(f,xs)
{
  return Array__MapIndexed$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__Map$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___ = (function(f,xs)
{
  return Array__MapIndexed$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__Map$Object__Object_Object__Object_ = (function(f,xs)
{
  return Array__MapIndexed$Object__Object_Object__Object_((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__Map$Object__String_Object__String = (function(f,xs)
{
  return Array__MapIndexed$Object__String_Object__String((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__Map$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___ = (function(f,xs)
{
  return Array__MapIndexed$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__Map$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___ = (function(f,xs)
{
  return Array__MapIndexed$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___((function(_arg1)
  {
    return (function(x)
    {
      return f(x);
    });
  }), xs);
});
Array__MapIndexed$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___ = (function(f,xs)
{
  var ys = Array__ZeroCreate$FSharpOption_1_Object___FSharpOption_1_Object___(Array__Length$FSharpAsync_1_Object___FSharpAsync_1_Object___(xs));
  for (var i = 0; i <= (Array__Length$FSharpAsync_1_Object___FSharpAsync_1_Object___(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__MapIndexed$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Object___Object___(Array__Length$FSharpOption_1_Object___FSharpOption_1_Object___(xs));
  for (var i = 0; i <= (Array__Length$FSharpOption_1_Object___FSharpOption_1_Object___(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__MapIndexed$Object__Object_Object__Object_ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Object_Object_(Array__Length$Object_Object_(xs));
  for (var i = 0; i <= (Array__Length$Object_Object_(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__MapIndexed$Object__String_Object__String = (function(f,xs)
{
  var ys = Array__ZeroCreate$String_String(Array__Length$Object_Object_(xs));
  for (var i = 0; i <= (Array__Length$Object_Object_(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__MapIndexed$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Tuple_2_String__Object___Tuple_2_String__Object___(Array__Length$Tuple_2_String__Double_Tuple_2_String__Double_(xs));
  for (var i = 0; i <= (Array__Length$Tuple_2_String__Double_Tuple_2_String__Double_(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__MapIndexed$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___ = (function(f,xs)
{
  var ys = Array__ZeroCreate$Object___Object___(Array__Length$Tuple_2_String__Object___Tuple_2_String__Object___(xs));
  for (var i = 0; i <= (Array__Length$Tuple_2_String__Object___Tuple_2_String__Object___(xs) - 1); i++)
  {
    ys[i] = f(i)(xs[i]);
    null;
  };
  return ys;
});
Array__ZeroCreate$Double_Double = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$FSharpOption_1_Object___FSharpOption_1_Object___ = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$Object_Object_ = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$Object___Object___ = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$String_String = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$Tuple_2_String__Double_Tuple_2_String__Double_ = (function(size)
{
  return new Array(size);;
});
Array__ZeroCreate$Tuple_2_String__Object___Tuple_2_String__Object___ = (function(size)
{
  return new Array(size);;
});
AsyncBuilder__Bind$DataTable__Unit_DataTable__Unit_ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Unit_Unit_(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_DataTable___ctor$DataTable_(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$FSharpSet_1_String__Tuple_2___FSharpSet_1_String__Tuple_2___ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Tuple_2___Tuple_2___(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_FSharpSet_1_String___ctor$FSharpSet_1_String_(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$Object____Tuple_2___Object____Tuple_2___ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Tuple_2___Tuple_2___(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_Object_____ctor$Object___(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$Object____Unit_Object____Unit_ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Unit_Unit_(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_Object_____ctor$Object___(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$Object______Object___Object______Object___ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Object___Object___(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_Object_______ctor$Object_____(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$String__FSharpSet_1_String_String_FSharpSet_1_String_ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$FSharpSet_1_String_FSharpSet_1_String_(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_String___ctor$String(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$String__Object___String_Object___ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Object___Object___(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_String___ctor$String(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$Tuple_2___1_DataTable_Tuple_2___1_DataTable_ = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$DataTable_DataTable_(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_Tuple_2___1__ctor$Tuple_2___1(cont, k.Aux)));
  }));
});
AsyncBuilder__Bind$Tuple_2____Tuple_2___1Tuple_2____Tuple_2___1 = (function(x,_arg1,f)
{
  var v = _arg1.Item;
  return (function(_f)
  {
    return Async_1_protectedCont$Tuple_2___1Tuple_2___1(_f);
  })((function(k)
  {
    var cont = (function(a)
    {
      var patternInput = f(a);
      var r = patternInput.Item;
      return r(k);
    });
    return v((new AsyncParams_1_Tuple_2_____ctor$Tuple_2___(cont, k.Aux)));
  }));
});
AsyncBuilder__Delay$DataTable_DataTable_ = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$DataTable_DataTable_(_f);
  })((function(k)
  {
    var _3488;
    var patternInput = f(_3488);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Delay$FSharpSet_1_String_FSharpSet_1_String_ = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$FSharpSet_1_String_FSharpSet_1_String_(_f);
  })((function(k)
  {
    var _1293;
    var patternInput = f(_1293);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Delay$Object___Object___ = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$Object___Object___(_f);
  })((function(k)
  {
    var _2163;
    var patternInput = f(_2163);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Delay$Tuple_2___1Tuple_2___1 = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$Tuple_2___1Tuple_2___1(_f);
  })((function(k)
  {
    var _3256;
    var patternInput = f(_3256);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Delay$Tuple_2___Tuple_2___ = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$Tuple_2___Tuple_2___(_f);
  })((function(k)
  {
    var _2994;
    var patternInput = f(_2994);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Delay$Unit_Unit_ = (function(x,f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$Unit_Unit_(_f);
  })((function(k)
  {
    var _1827;
    var patternInput = f(_1827);
    var r = patternInput.Item;
    return r(k);
  }));
});
AsyncBuilder__Return$DataTable_DataTable_ = (function(x,v)
{
  return (function(f)
  {
    return Async_1_protectedCont$DataTable_DataTable_(f);
  })((function(k)
  {
    return Async_1_invokeCont$DataTable_DataTable_(k, v);
  }));
});
AsyncBuilder__Return$FSharpSet_1_String_FSharpSet_1_String_ = (function(x,v)
{
  return (function(f)
  {
    return Async_1_protectedCont$FSharpSet_1_String_FSharpSet_1_String_(f);
  })((function(k)
  {
    return Async_1_invokeCont$FSharpSet_1_String_FSharpSet_1_String_(k, v);
  }));
});
AsyncBuilder__Return$Object___Object___ = (function(x,v)
{
  return (function(f)
  {
    return Async_1_protectedCont$Object___Object___(f);
  })((function(k)
  {
    return Async_1_invokeCont$Object___Object___(k, v);
  }));
});
AsyncBuilder__Return$Tuple_2___1Tuple_2___1 = (function(x,v)
{
  return (function(f)
  {
    return Async_1_protectedCont$Tuple_2___1Tuple_2___1(f);
  })((function(k)
  {
    return Async_1_invokeCont$Tuple_2___1Tuple_2___1(k, v);
  }));
});
AsyncBuilder__Return$Tuple_2___Tuple_2___ = (function(x,v)
{
  return (function(f)
  {
    return Async_1_protectedCont$Tuple_2___Tuple_2___(f);
  })((function(k)
  {
    return Async_1_invokeCont$Tuple_2___Tuple_2___(k, v);
  }));
});
AsyncBuilder__TryWith$Unit_Unit_ = (function(x,_arg2,catchFunction)
{
  var v = _arg2.Item;
  return (function(f)
  {
    return Async_1_protectedCont$Unit_Unit_(f);
  })((function(k)
  {
    CancellationToken__ThrowIfCancellationRequested$(k.Aux.CancellationToken);
    var inputRecord = k.Aux;
    var ExceptionCont = (function(ex)
    {
      return k.Cont(catchFunction(ex));
    });
    var Aux = (new AsyncParamsAux___ctor$(inputRecord.StackCounter, ExceptionCont, inputRecord.CancelledCont, inputRecord.CancellationToken));
    return v((new AsyncParams_1_Unit___ctor$Unit_(k.Cont, Aux)));
  }));
});
AsyncBuilder__Zero$ = (function(x,unitVar1)
{
  return (function(f)
  {
    return Async_1_protectedCont$Unit_Unit_(f);
  })((function(k)
  {
    var _1794;
    return Async_1_invokeCont$Unit_Unit_(k, _1794);
  }));
});
AsyncBuilder___ctor$ = (function(unitVar0)
{
  ;
});
AsyncParamsAux___ctor$ = (function(StackCounter,ExceptionCont,CancelledCont,CancellationToken)
{
  var __this = this;
  __this.StackCounter = StackCounter;
  __this.ExceptionCont = ExceptionCont;
  __this.CancelledCont = CancelledCont;
  __this.CancellationToken = CancellationToken;
});
AsyncParams_1_DataTable___ctor$DataTable_ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_FSharpSet_1_String___ctor$FSharpSet_1_String_ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_Object_______ctor$Object_____ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_Object_____ctor$Object___ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_String___ctor$String = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_Tuple_2___1__ctor$Tuple_2___1 = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_Tuple_2_____ctor$Tuple_2___ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
AsyncParams_1_Unit___ctor$Unit_ = (function(Cont,Aux)
{
  var __this = this;
  __this.Cont = Cont;
  __this.Aux = Aux;
});
Async_1_DataTable__ContDataTable_ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_FSharpSet_1_String__ContFSharpSet_1_String_ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_Object____ContObject___ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_Object______ContObject_____ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_String__ContString = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_Tuple_2___1_ContTuple_2___1 = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_Tuple_2____ContTuple_2___ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_Unit__ContUnit_ = (function(Item)
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Cont";
  __this.Item = Item;
});
Async_1_get_async$ = (function()
{
  return (new AsyncBuilder___ctor$());
});
Async_1_invokeCont$DataTable_DataTable_ = (function(k,value)
{
  return k.Cont(value);
});
Async_1_invokeCont$FSharpSet_1_String_FSharpSet_1_String_ = (function(k,value)
{
  return k.Cont(value);
});
Async_1_invokeCont$Object___Object___ = (function(k,value)
{
  return k.Cont(value);
});
Async_1_invokeCont$Tuple_2___1Tuple_2___1 = (function(k,value)
{
  return k.Cont(value);
});
Async_1_invokeCont$Tuple_2___Tuple_2___ = (function(k,value)
{
  return k.Cont(value);
});
Async_1_invokeCont$Unit_Unit_ = (function(k,value)
{
  return k.Cont(value);
});
Async_1_protectedCont$DataTable_DataTable_ = (function(f)
{
  return (new Async_1_DataTable__ContDataTable_((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$FSharpSet_1_String_FSharpSet_1_String_ = (function(f)
{
  return (new Async_1_FSharpSet_1_String__ContFSharpSet_1_String_((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$Object___Object___ = (function(f)
{
  return (new Async_1_Object____ContObject___((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$Object_____Object_____ = (function(f)
{
  return (new Async_1_Object______ContObject_____((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$String_String = (function(f)
{
  return (new Async_1_String__ContString((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$Tuple_2___1Tuple_2___1 = (function(f)
{
  return (new Async_1_Tuple_2___1_ContTuple_2___1((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$Tuple_2___Tuple_2___ = (function(f)
{
  return (new Async_1_Tuple_2____ContTuple_2___((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_1_protectedCont$Unit_Unit_ = (function(f)
{
  return (new Async_1_Unit__ContUnit_((function(args)
  {
    CancellationToken__ThrowIfCancellationRequested$(args.Aux.CancellationToken);
    args.Aux.StackCounter.contents = (args.Aux.StackCounter.contents + 1);
    null;
    if ((args.Aux.StackCounter.contents > 1000)) 
    {
      args.Aux.StackCounter.contents = 0;
      null;
      return window.setTimeout((function(unitVar0)
      {
        try
        {
          return f(args);
        }
        catch(ex){
          return args.Aux.ExceptionCont(ex);
        };
      }), 1.000000);
    }
    else
    {
      try
      {
        return f(args);
      }
      catch(ex){
        return args.Aux.ExceptionCont(ex);
      };
    };
  })));
});
Async_2_PseudoParallel$Object___Object___ = (function(work)
{
  return Async__FromContinuations$Object_____Object_____((function(tupledArg)
  {
    var cont = tupledArg.Items[0.000000];
    var econt = tupledArg.Items[1.000000];
    var ccont = tupledArg.Items[2.000000];
    var workItems = Seq__ToArray$FSharpAsync_1_Object___FSharpAsync_1_Object___(work);
    if ((Array__BoxedLength$(workItems) == 0)) 
    {
      return cont([]);
    }
    else
    {
      var mapping = (function(_arg1)
      {
        return {Tag: 0.000000};
      });
      var results = (function(array)
      {
        return Array__Map$FSharpAsync_1_Object____FSharpOption_1_Object___FSharpAsync_1_Object____FSharpOption_1_Object___(mapping, array);
      })(workItems);
      var count = {contents: 0};
      for (var _1834 = 0; _1834 <= (Array__BoxedLength$(workItems) - 1); _1834++)
      {
        (function(i)
        {
          (function(arg00)
          {
            return Async__StartImmediate$(arg00, {Tag: 0.000000});
          })((function(builder_)
          {
            return AsyncBuilder__Delay$Unit_Unit_(builder_, (function(unitVar)
            {
              return AsyncBuilder__Bind$Object____Unit_Object____Unit_(builder_, workItems[i], (function(_arg1)
              {
                var res = _arg1;
                results[i] = {Tag: 1.000000, Value: res};
                null;
                count.contents = (count.contents + 1);
                null;
                if ((count.contents == Array__BoxedLength$(results))) 
                {
                  cont(Array__Map$FSharpOption_1_Object____Object___FSharpOption_1_Object____Object___((function(option)
                  {
                    return Option__GetValue$Object___Object___(option);
                  }), results));
                  return AsyncBuilder__Zero$(builder_);
                }
                else
                {
                  return AsyncBuilder__Zero$(builder_);
                };
              }));
            }));
          })(Async_1_get_async$()));
        })(_1834);
      };
    };
  }));
});
Async__FromContinuations$Object_____Object_____ = (function(f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$Object_____Object_____(_f);
  })((function(k)
  {
    return f((new TupleFSharpFunc_2_Object______Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_(k.Cont, k.Aux.ExceptionCont, k.Aux.CancelledCont)));
  }));
});
Async__FromContinuations$String_String = (function(f)
{
  return (function(_f)
  {
    return Async_1_protectedCont$String_String(_f);
  })((function(k)
  {
    return f((new TupleFSharpFunc_2_String__Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_(k.Cont, k.Aux.ExceptionCont, k.Aux.CancelledCont)));
  }));
});
Async__StartImmediate$ = (function(workflow,cancellationToken)
{
  var _1587;
  if ((cancellationToken.Tag == 1.000000)) 
  {
    var v = Option__GetValue$CancellationToken_CancellationToken_(cancellationToken);
    _1587 = v;
  }
  else
  {
    _1587 = (new CancellationToken___ctor$({Tag: 0.000000}));
  };
  var token = _1587;
  var f = workflow.Item;
  var aux = (new AsyncParamsAux___ctor$({contents: 0}, (function(value)
  {
    var ignored0 = value;
  }), (function(value)
  {
    var ignored0 = value;
  }), token));
  return f((new AsyncParams_1_Unit___ctor$Unit_((function(value)
  {
    var ignored0 = value;
  }), aux)));
});
CancellationToken__ThrowIfCancellationRequested$ = (function(x,unitVar1)
{
  var matchValue = x.Cell;
  if ((matchValue.Tag == 1.000000)) 
  {
    var cell = Option__GetValue$FSharpRef_1_Boolean_FSharpRef_1_Boolean_(matchValue);
    if (cell.contents) 
    {
      var _cell = Option__GetValue$FSharpRef_1_Boolean_FSharpRef_1_Boolean_(matchValue);
      throw ("OperationCancelledException");
      return null;
    }
    else
    {
      ;
    };
  }
  else
  {
    ;
  };
});
CancellationToken___ctor$ = (function(Cell)
{
  var __this = this;
  __this.Cell = Cell;
});
ChartColorAxis___ctor$ = (function(minValue,maxValue,values,colors,legend)
{
  var __this = this;
  __this.minValue = minValue;
  __this.maxValue = maxValue;
  __this.values = values;
  __this.colors = colors;
  __this.legend = legend;
});
ChartDataModule__oneKeyValue$String_String = (function(keyType,v)
{
  return (new ChartData___ctor$((function(builder_)
  {
    return AsyncBuilder__Delay$DataTable_DataTable_(builder_, (function(unitVar)
    {
      var data = (new google.visualization.DataTable());
      (function(value)
      {
        var ignored0 = value;
      })((data.addColumn(keyType, v.keyName)));
      (function(value)
      {
        var ignored0 = value;
      })((data.addColumn("number", v.seriesName)));
      return AsyncBuilder__Bind$Tuple_2___1_DataTable_Tuple_2___1_DataTable_(builder_, Operations__series_2_mapPairs$String__Double__Object___String_Double_Object___(v, (function(k)
      {
        return (function(_v)
        {
          return [k, _v];
        });
      })).data, (function(_arg1)
      {
        var vals = _arg1;
        var mapping = (function(tuple)
        {
          return tuple.Items[1.000000];
        });
        (function(value)
        {
          var ignored0 = value;
        })((function(arg00)
        {
          return (data.addRows(arg00));
        })((function(array)
        {
          return Array__Map$Tuple_2_String__Object____Object___Tuple_2_String__Object____Object___(mapping, array);
        })(vals)));
        return AsyncBuilder__Return$DataTable_DataTable_(builder_, data);
      }));
    }));
  })(Async_1_get_async$())));
});
ChartData___ctor$ = (function(data)
{
  var __this = this;
  __this.data = data;
});
CreateEnumerable_1_FSharpAsync_1_Object_____ctor$FSharpAsync_1_Object___ = (function(factory)
{
  var __this = this;
  __this.factory = factory;
});
CreateEnumerable_1_Int32___ctor$Int32 = (function(factory)
{
  var __this = this;
  __this.factory = factory;
});
CreateEnumerable_1_Object_____ctor$Object___ = (function(factory)
{
  var __this = this;
  __this.factory = factory;
});
CreateEnumerable_1_String___ctor$String = (function(factory)
{
  var __this = this;
  __this.factory = factory;
});
DateTime__Parse$ = (function(s)
{
  return DateTime__createUnsafe$(s, 2);
});
DateTime__createUnsafe$ = (function(value,kind)
{
  var date = value == null ? new Date() : new Date(value);
  if (isNaN(date)) { throw "The string was not recognized as a valid DateTime." }
  date.kind = kind;
  return date;
});
Extensions__GeoChartOptions_get_empty_Static$ = (function(unitVar0)
{
  return (new GeoChartOptions___ctor$(undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined));
});
FSharpString__Concat$ = (function(sep,strings)
{
  return Seq__ToArray$String_String(strings).join(sep);
});
GenericComparer_1_String___ctor$String = (function(unitVar0)
{
  ;
});
GenericConstants__One$Int32_Int32 = (function()
{
  return 1;;
});
GenericConstants__Zero$Int32_Int32 = (function()
{
  return 0;;
});
GeoChartOptions___ctor$ = (function(backgroundColor,colorAxis,datalessRegionColor,displayMode,enableRegionInteractivity,height,keepAspectRatio,legend,region,magnifyingGlass,markerOpacity,resolution,sizeAxis,tooltip,width)
{
  var __this = this;
  __this.backgroundColor = backgroundColor;
  __this.colorAxis = colorAxis;
  __this.datalessRegionColor = datalessRegionColor;
  __this.displayMode = displayMode;
  __this.enableRegionInteractivity = enableRegionInteractivity;
  __this.height = height;
  __this.keepAspectRatio = keepAspectRatio;
  __this.legend = legend;
  __this.region = region;
  __this.magnifyingGlass = magnifyingGlass;
  __this.markerOpacity = markerOpacity;
  __this.resolution = resolution;
  __this.sizeAxis = sizeAxis;
  __this.tooltip = tooltip;
  __this.width = width;
});
Geo___ctor$ = (function(data,typeName,options)
{
  var __this = this;
  __this.data = data;
  __this.typeName = typeName;
  __this.options = options;
});
Geo__colorAxis$ = (function(x,minValue,maxValue,values,colors)
{
  var o = x.options.colorAxis;
  var newNested = (new ChartColorAxis___ctor$(Helpers_1_right$ChartColorAxis__Double_ChartColorAxis__Double(o, "minValue", minValue), Helpers_1_right$ChartColorAxis__Double_ChartColorAxis__Double(o, "maxValue", maxValue), Helpers_1_right$ChartColorAxis__Double___ChartColorAxis__Double___(o, "values", Option__Map$IEnumerable_1_Double__Double___IEnumerable_1_Double__Double___((function(source)
  {
    return Seq__ToArray$Double_Double(source);
  }), values)), Helpers_1_right$ChartColorAxis__String___ChartColorAxis__String___(o, "colors", Option__Map$IEnumerable_1_String__String___IEnumerable_1_String__String___((function(source)
  {
    return Seq__ToArray$String_String(source);
  }), colors)), Helpers_1_copy$ChartColorAxis__ChartLegend_ChartColorAxis__ChartLegend_(o, "legend")));
  var inputRecord = x.options;
  var options = (new GeoChartOptions___ctor$(inputRecord.backgroundColor, newNested, inputRecord.datalessRegionColor, inputRecord.displayMode, inputRecord.enableRegionInteractivity, inputRecord.height, inputRecord.keepAspectRatio, inputRecord.legend, inputRecord.region, inputRecord.magnifyingGlass, inputRecord.markerOpacity, inputRecord.resolution, inputRecord.sizeAxis, inputRecord.tooltip, inputRecord.width));
  return (new Geo___ctor$(x.data, x.typeName, options));
});
Geo__show$ = (function(x,unitVar1)
{
  return Helpers_1_showChart$Geo_Geo_(x);
});
Helpers_1_copy$ChartColorAxis__ChartLegend_ChartColorAxis__ChartLegend_ = (function(o,prop)
{
  if (o==null) 
  {
    return undefined;
  }
  else
  {
    return o[prop];
  };
});
Helpers_1_drawChart$Object_Object_ = (function(chart,data)
{
  drawChart(chart, data, outputElementID, blockCallback);;
});
Helpers_1_right$ChartColorAxis__Double_ChartColorAxis__Double = (function(o,prop,newValue)
{
  if ((newValue.Tag == 1.000000)) 
  {
    var a = Option__GetValue$Double_Double(newValue);
    return a;
  }
  else
  {
    if (o==null) 
    {
      return undefined;
    }
    else
    {
      return o[prop];
    };
  };
});
Helpers_1_right$ChartColorAxis__Double___ChartColorAxis__Double___ = (function(o,prop,newValue)
{
  if ((newValue.Tag == 1.000000)) 
  {
    var a = Option__GetValue$Double___Double___(newValue);
    return a;
  }
  else
  {
    if (o==null) 
    {
      return undefined;
    }
    else
    {
      return o[prop];
    };
  };
});
Helpers_1_right$ChartColorAxis__String___ChartColorAxis__String___ = (function(o,prop,newValue)
{
  if ((newValue.Tag == 1.000000)) 
  {
    var a = Option__GetValue$String___String___(newValue);
    return a;
  }
  else
  {
    if (o==null) 
    {
      return undefined;
    }
    else
    {
      return o[prop];
    };
  };
});
Helpers_1_showChart$Geo_Geo_ = (function(chart)
{
  return (function(arg00)
  {
    return Async__StartImmediate$(arg00, {Tag: 0.000000});
  })((function(builder_)
  {
    return AsyncBuilder__Delay$Unit_Unit_(builder_, (function(unitVar)
    {
      return AsyncBuilder__TryWith$Unit_Unit_(builder_, AsyncBuilder__Delay$Unit_Unit_(builder_, (function(_unitVar)
      {
        return AsyncBuilder__Bind$DataTable__Unit_DataTable__Unit_(builder_, chart["data"].data, (function(_arg1)
        {
          var dt = _arg1;
          Helpers_1_drawChart$Object_Object_(chart, dt);
          return AsyncBuilder__Zero$(builder_);
        }));
      })), (function(_arg2)
      {
        var e = _arg2;
        ((window.window).alert(("SOmething went wrong: " + e)));
        return AsyncBuilder__Zero$(builder_);
      }));
    }));
  })(Async_1_get_async$()));
});
Helpers__encodeURIComponent$ = (function(s)
{
  return encodeURIComponent(s);;
});
Helpers__getGlobal$FSharpSet_1_String_FSharpSet_1_String_ = (function(name)
{
  return window[name];;
});
Helpers__getGlobal$Object_Object_ = (function(name)
{
  return window[name];;
});
Helpers__getJSONPrefix$ = (function(url,callback)
{
  
  $.ajax({
    url: url,
    dataType: 'jsonp',
    jsonp: 'prefix',
    jsonpCallback: 'wb_prefix_' + Math.random().toString().substr(2),
    error: function(jqXHR, textStatus, errorThrown){
        console.log(textStatus + errorThrown);
    },
    success: function(data){
        callback(data);
    }
  });
;
});
Helpers__getProperty$Object_Object_ = (function(obj,name)
{
  return obj[name];;
});
Helpers__isArray$ = (function(o)
{
  return Array.isArray(o);;
});
Helpers__isNull$ = (function(o)
{
  return o==null;;
});
Helpers__jsTypeOf$ = (function(o)
{
  return typeof(o);;
});
Helpers__setGlobal$FSharpSet_1_String_FSharpSet_1_String_ = (function(name,value)
{
  window[name] = value;;
});
Helpers__tryGetGlobal$Object_Object_ = (function(name)
{
  var v = Helpers__getGlobal$Object_Object_(name);
  if (Helpers__isNull$(v)) 
  {
    return {Tag: 0.000000};
  }
  else
  {
    return {Tag: 1.000000, Value: v};
  };
});
Json__getArrayMemberByTag$ = (function(tag,input)
{
  return Json__getArrayMembersByTag$(tag, input)[0];
});
Json__getArrayMembersByTag$ = (function(tag,input)
{
  var chooser = (function(value)
  {
    var matchValue = (new TupleString_String(tag, Helpers__jsTypeOf$(value)));
    if ((matchValue.Items[0.000000] == "Boolean")) 
    {
      if ((matchValue.Items[1.000000] == "boolean")) 
      {
        return {Tag: 1.000000, Value: value};
      }
      else
      {
        if ((matchValue.Items[0.000000] == "Record")) 
        {
          if ((matchValue.Items[1.000000] == "object")) 
          {
            if ((!Helpers__isArray$(value))) 
            {
              return {Tag: 1.000000, Value: value};
            }
            else
            {
              return {Tag: 0.000000};
            };
          }
          else
          {
            return {Tag: 0.000000};
          };
        }
        else
        {
          return {Tag: 0.000000};
        };
      };
    }
    else
    {
      if ((matchValue.Items[0.000000] == "Number")) 
      {
        if ((matchValue.Items[1.000000] == "number")) 
        {
          return {Tag: 1.000000, Value: value};
        }
        else
        {
          if ((matchValue.Items[0.000000] == "Record")) 
          {
            if ((matchValue.Items[1.000000] == "object")) 
            {
              if ((!Helpers__isArray$(value))) 
              {
                return {Tag: 1.000000, Value: value};
              }
              else
              {
                return {Tag: 0.000000};
              };
            }
            else
            {
              return {Tag: 0.000000};
            };
          }
          else
          {
            return {Tag: 0.000000};
          };
        };
      }
      else
      {
        if ((matchValue.Items[0.000000] == "String")) 
        {
          if ((matchValue.Items[1.000000] == "string")) 
          {
            return {Tag: 1.000000, Value: value};
          }
          else
          {
            if ((matchValue.Items[0.000000] == "Record")) 
            {
              if ((matchValue.Items[1.000000] == "object")) 
              {
                if ((!Helpers__isArray$(value))) 
                {
                  return {Tag: 1.000000, Value: value};
                }
                else
                {
                  return {Tag: 0.000000};
                };
              }
              else
              {
                return {Tag: 0.000000};
              };
            }
            else
            {
              return {Tag: 0.000000};
            };
          };
        }
        else
        {
          if ((matchValue.Items[0.000000] == "DateTime")) 
          {
            if ((matchValue.Items[1.000000] == "string")) 
            {
              return {Tag: 1.000000, Value: DateTime__Parse$(value)};
            }
            else
            {
              if ((matchValue.Items[0.000000] == "Record")) 
              {
                if ((matchValue.Items[1.000000] == "object")) 
                {
                  if ((!Helpers__isArray$(value))) 
                  {
                    return {Tag: 1.000000, Value: value};
                  }
                  else
                  {
                    return {Tag: 0.000000};
                  };
                }
                else
                {
                  return {Tag: 0.000000};
                };
              }
              else
              {
                return {Tag: 0.000000};
              };
            };
          }
          else
          {
            if ((matchValue.Items[0.000000] == "Array")) 
            {
              if (Helpers__isArray$(value)) 
              {
                return {Tag: 1.000000, Value: value};
              }
              else
              {
                if ((matchValue.Items[0.000000] == "Record")) 
                {
                  if ((matchValue.Items[1.000000] == "object")) 
                  {
                    if ((!Helpers__isArray$(value))) 
                    {
                      return {Tag: 1.000000, Value: value};
                    }
                    else
                    {
                      return {Tag: 0.000000};
                    };
                  }
                  else
                  {
                    return {Tag: 0.000000};
                  };
                }
                else
                {
                  return {Tag: 0.000000};
                };
              };
            }
            else
            {
              if ((matchValue.Items[0.000000] == "Record")) 
              {
                if ((matchValue.Items[1.000000] == "object")) 
                {
                  if ((!Helpers__isArray$(value))) 
                  {
                    return {Tag: 1.000000, Value: value};
                  }
                  else
                  {
                    return {Tag: 0.000000};
                  };
                }
                else
                {
                  return {Tag: 0.000000};
                };
              }
              else
              {
                return {Tag: 0.000000};
              };
            };
          };
        };
      };
    };
  });
  return (function(array)
  {
    return Array__Choose$Object__Object_Object__Object_(chooser, array);
  })(input);
});
LanguagePrimitives__UnboxGeneric$IDisposable_IDisposable_ = (function(x)
{
  return x;;
});
List__CreateCons$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(x,xs)
{
  return (new list_1_FSharpAsync_1_Object____ConsFSharpAsync_1_Object___(x, xs));
});
List__CreateCons$String_String = (function(x,xs)
{
  return (new list_1_String__ConsString(x, xs));
});
List__CreateCons$Tuple_2_String__String_Tuple_2_String__String_ = (function(x,xs)
{
  return (new list_1_Tuple_2_String__String__ConsTuple_2_String__String_(x, xs));
});
List__Empty$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function()
{
  return (new list_1_FSharpAsync_1_Object____NilFSharpAsync_1_Object___());
});
List__Empty$String_String = (function()
{
  return (new list_1_String__NilString());
});
List__Empty$Tuple_2_String__String_Tuple_2_String__String_ = (function()
{
  return (new list_1_Tuple_2_String__String__NilTuple_2_String__String_());
});
List__Fold$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___ = (function(f,seed,xs)
{
  return List__FoldIndexed$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___((function(_arg1)
  {
    return (function(acc)
    {
      return (function(x)
      {
        return f(acc)(x);
      });
    });
  }), seed, xs);
});
List__Fold$String__list_1_String_String_list_1_String_ = (function(f,seed,xs)
{
  return List__FoldIndexed$String__list_1_String_String_list_1_String_((function(_arg1)
  {
    return (function(acc)
    {
      return (function(x)
      {
        return f(acc)(x);
      });
    });
  }), seed, xs);
});
List__Fold$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_ = (function(f,seed,xs)
{
  return List__FoldIndexed$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_((function(_arg1)
  {
    return (function(acc)
    {
      return (function(x)
      {
        return f(acc)(x);
      });
    });
  }), seed, xs);
});
List__FoldIndexed$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___ = (function(f,seed,xs)
{
  return List__FoldIndexedAux$list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___(f, 0, seed, xs);
});
List__FoldIndexed$String__list_1_String_String_list_1_String_ = (function(f,seed,xs)
{
  return List__FoldIndexedAux$list_1_String__String_list_1_String__String(f, 0, seed, xs);
});
List__FoldIndexed$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_ = (function(f,seed,xs)
{
  return List__FoldIndexedAux$list_1_String__Tuple_2_String__String_list_1_String__Tuple_2_String__String_(f, 0, seed, xs);
});
List__FoldIndexedAux$list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___ = (function(f,i,acc,_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return List__FoldIndexedAux$list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___list_1_FSharpAsync_1_Object____FSharpAsync_1_Object___(f, (i + 1), f(i)(acc)(x), xs);
  }
  else
  {
    return acc;
  };
});
List__FoldIndexedAux$list_1_String__String_list_1_String__String = (function(f,i,acc,_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return List__FoldIndexedAux$list_1_String__String_list_1_String__String(f, (i + 1), f(i)(acc)(x), xs);
  }
  else
  {
    return acc;
  };
});
List__FoldIndexedAux$list_1_String__Tuple_2_String__String_list_1_String__Tuple_2_String__String_ = (function(f,i,acc,_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return List__FoldIndexedAux$list_1_String__Tuple_2_String__String_list_1_String__Tuple_2_String__String_(f, (i + 1), f(i)(acc)(x), xs);
  }
  else
  {
    return acc;
  };
});
List__Head$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return x;
  }
  else
  {
    throw ("List was empty");
    return null;
  };
});
List__Head$String_String = (function(_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return x;
  }
  else
  {
    throw ("List was empty");
    return null;
  };
});
List__Map$String__String_String_String = (function(f,xs)
{
  return (function(_xs)
  {
    return List__Reverse$String_String(_xs);
  })(List__Fold$String__list_1_String_String_list_1_String_((function(acc)
  {
    return (function(x)
    {
      return (new list_1_String__ConsString(f(x), acc));
    });
  }), (new list_1_String__NilString()), xs));
});
List__Map$Tuple_2_String__String__String_Tuple_2_String__String__String = (function(f,xs)
{
  return (function(_xs)
  {
    return List__Reverse$String_String(_xs);
  })(List__Fold$Tuple_2_String__String__list_1_String_Tuple_2_String__String__list_1_String_((function(acc)
  {
    return (function(x)
    {
      return (new list_1_String__ConsString(f(x), acc));
    });
  }), (new list_1_String__NilString()), xs));
});
List__Reverse$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  return List__Fold$FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___FSharpAsync_1_Object____list_1_FSharpAsync_1_Object___((function(acc)
  {
    return (function(x)
    {
      return (new list_1_FSharpAsync_1_Object____ConsFSharpAsync_1_Object___(x, acc));
    });
  }), (new list_1_FSharpAsync_1_Object____NilFSharpAsync_1_Object___()), xs);
});
List__Reverse$String_String = (function(xs)
{
  return List__Fold$String__list_1_String_String_list_1_String_((function(acc)
  {
    return (function(x)
    {
      return (new list_1_String__ConsString(x, acc));
    });
  }), (new list_1_String__NilString()), xs);
});
List__Tail$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return xs;
  }
  else
  {
    throw ("List was empty");
    return null;
  };
});
List__Tail$String_String = (function(_arg1)
{
  if ((_arg1.Tag == 1.000000)) 
  {
    var xs = _arg1.Item2;
    var x = _arg1.Item1;
    return xs;
  }
  else
  {
    throw ("List was empty");
    return null;
  };
});
Operations__lift$String__Double__String__Object___String_Double_String_Object___ = (function(f,s)
{
  return series_2_String__Double__set$String__Object___String_Double_String_Object___(s, (function(builder_)
  {
    return AsyncBuilder__Delay$Tuple_2___1Tuple_2___1(builder_, (function(unitVar)
    {
      return AsyncBuilder__Bind$Tuple_2____Tuple_2___1Tuple_2____Tuple_2___1(builder_, s.data, (function(_arg1)
      {
        var vs = _arg1;
        return AsyncBuilder__Return$Tuple_2___1Tuple_2___1(builder_, f(vs));
      }));
    }));
  })(Async_1_get_async$()), {Tag: 0.000000}, {Tag: 0.000000}, {Tag: 0.000000});
});
Operations__series_2_mapPairs$String__Double__Object___String_Double_Object___ = (function(s,f)
{
  var mapping = (function(tupledArg)
  {
    var k = tupledArg.Items[0.000000];
    var v = tupledArg.Items[1.000000];
    return (new TupleString_Object___(k, f(k)(v)));
  });
  var _f = (function(array)
  {
    return Array__Map$Tuple_2_String__Double__Tuple_2_String__Object___Tuple_2_String__Double__Tuple_2_String__Object___(mapping, array);
  });
  return (function(_s)
  {
    return Operations__lift$String__Double__String__Object___String_Double_String_Object___(_f, _s);
  })(s);
});
Option__GetValue$CancellationToken_CancellationToken_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Decimal_Decimal = (function(option)
{
  return option.Value;;
});
Option__GetValue$Double_Double = (function(option)
{
  return option.Value;;
});
Option__GetValue$Double___Double___ = (function(option)
{
  return option.Value;;
});
Option__GetValue$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___ = (function(option)
{
  return option.Value;;
});
Option__GetValue$FSharpList_1_String_FSharpList_1_String_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$FSharpRef_1_Boolean_FSharpRef_1_Boolean_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$IEnumerable_1_Double_IEnumerable_1_Double_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$IEnumerable_1_String_IEnumerable_1_String_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$IEnumerator_1_Int32_IEnumerator_1_Int32_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Int32_Int32 = (function(option)
{
  return option.Value;;
});
Option__GetValue$Object_Object_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Object___Object___ = (function(option)
{
  return option.Value;;
});
Option__GetValue$String_String = (function(option)
{
  return option.Value;;
});
Option__GetValue$String___String___ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_Int32__Int32_Tuple_2_Int32__Int32_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_Object____Int32_Tuple_2_Object____Int32_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_String__Double_Tuple_2_String__Double_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_String__FSharpList_1_String_Tuple_2_String__FSharpList_1_String_ = (function(option)
{
  return option.Value;;
});
Option__GetValue$Tuple_2_String__Int32_Tuple_2_String__Int32_ = (function(option)
{
  return option.Value;;
});
Option__IsNone$Decimal_Decimal = (function(option)
{
  return (!Option__IsSome$Decimal_Decimal(option));
});
Option__IsSome$Decimal_Decimal = (function(option)
{
  return ((option.Tag == 1.000000) && true);
});
Option__IsSome$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___ = (function(option)
{
  return ((option.Tag == 1.000000) && true);
});
Option__IsSome$FSharpList_1_String_FSharpList_1_String_ = (function(option)
{
  return ((option.Tag == 1.000000) && true);
});
Option__IsSome$IEnumerator_1_Int32_IEnumerator_1_Int32_ = (function(option)
{
  return ((option.Tag == 1.000000) && true);
});
Option__IsSome$Int32_Int32 = (function(option)
{
  return ((option.Tag == 1.000000) && true);
});
Option__Map$IEnumerable_1_Double__Double___IEnumerable_1_Double__Double___ = (function(f,inp)
{
  if ((inp.Tag == 1.000000)) 
  {
    var x = Option__GetValue$IEnumerable_1_Double_IEnumerable_1_Double_(inp);
    return {Tag: 1.000000, Value: f(x)};
  }
  else
  {
    return {Tag: 0.000000};
  };
});
Option__Map$IEnumerable_1_String__String___IEnumerable_1_String__String___ = (function(f,inp)
{
  if ((inp.Tag == 1.000000)) 
  {
    var x = Option__GetValue$IEnumerable_1_String_IEnumerable_1_String_(inp);
    return {Tag: 1.000000, Value: f(x)};
  }
  else
  {
    return {Tag: 0.000000};
  };
});
Range__customStep$Int32__Int32_Int32_Int32 = (function(first,stepping,last)
{
  var zero = GenericConstants__Zero$Int32_Int32();
  var f = (function(x)
  {
    if ((((stepping > zero) && (x <= last)) || ((stepping < zero) && (x >= last)))) 
    {
      return {Tag: 1.000000, Value: (new TupleInt32_Int32(x, (x + stepping)))};
    }
    else
    {
      return {Tag: 0.000000};
    };
  });
  return (function(seed)
  {
    return Seq__Unfold$Int32__Int32_Int32_Int32(f, seed);
  })(first);
});
Range__oneStep$Int32_Int32 = (function(first,last)
{
  return Range__customStep$Int32__Int32_Int32_Int32(first, GenericConstants__One$Int32_Int32(), last);
});
Seq__Delay$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(f)
{
  return Seq__FromFactory$FSharpAsync_1_Object___FSharpAsync_1_Object___((function(unitVar0)
  {
    var _2279;
    return Seq__Enumerator$FSharpAsync_1_Object___FSharpAsync_1_Object___(f(_2279));
  }));
});
Seq__Enumerator$Double_Double = (function(xs)
{
  return xs.GetEnumerator();
});
Seq__Enumerator$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  return xs.GetEnumerator();
});
Seq__Enumerator$Int32_Int32 = (function(xs)
{
  return xs.GetEnumerator();
});
Seq__Enumerator$Object___Object___ = (function(xs)
{
  return xs.GetEnumerator();
});
Seq__Enumerator$String_String = (function(xs)
{
  return xs.GetEnumerator();
});
Seq__Fold$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___ = (function(f,seed,xs)
{
  return Seq__FoldIndexed$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___((function(_arg1)
  {
    return (function(acc)
    {
      return (function(x)
      {
        return f(acc)(x);
      });
    });
  }), seed, xs);
});
Seq__FoldIndexed$Double__Unit_Double_Unit_ = (function(f,seed,xs)
{
  return Seq__FoldIndexedAux$Unit__Double_Unit__Double(f, seed, Seq__Enumerator$Double_Double(xs));
});
Seq__FoldIndexed$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___ = (function(f,seed,xs)
{
  return Seq__FoldIndexedAux$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___(f, seed, Seq__Enumerator$FSharpAsync_1_Object___FSharpAsync_1_Object___(xs));
});
Seq__FoldIndexed$FSharpAsync_1_Object____Unit_FSharpAsync_1_Object____Unit_ = (function(f,seed,xs)
{
  return Seq__FoldIndexedAux$Unit__FSharpAsync_1_Object___Unit__FSharpAsync_1_Object___(f, seed, Seq__Enumerator$FSharpAsync_1_Object___FSharpAsync_1_Object___(xs));
});
Seq__FoldIndexed$Object____Unit_Object____Unit_ = (function(f,seed,xs)
{
  return Seq__FoldIndexedAux$Unit__Object___Unit__Object___(f, seed, Seq__Enumerator$Object___Object___(xs));
});
Seq__FoldIndexed$String__Unit_String_Unit_ = (function(f,seed,xs)
{
  return Seq__FoldIndexedAux$Unit__String_Unit__String(f, seed, Seq__Enumerator$String_String(xs));
});
Seq__FoldIndexedAux$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___ = (function(f,acc,xs)
{
  var i = {contents: 0};
  var _acc = {contents: acc};
  while (xs.MoveNext())
  {
    _acc.contents = f(i.contents)(_acc.contents)(xs.get_Current());
    null;
    i.contents = (i.contents + 1);
    null;
  };
  return _acc.contents;
});
Seq__FoldIndexedAux$Unit__Double_Unit__Double = (function(f,acc,xs)
{
  var i = {contents: 0};
  var _acc = {contents: acc};
  while (xs.MoveNext())
  {
    _acc.contents = f(i.contents)(_acc.contents)(xs.get_Current());
    null;
    i.contents = (i.contents + 1);
    null;
  };
  return _acc.contents;
});
Seq__FoldIndexedAux$Unit__FSharpAsync_1_Object___Unit__FSharpAsync_1_Object___ = (function(f,acc,xs)
{
  var i = {contents: 0};
  var _acc = {contents: acc};
  while (xs.MoveNext())
  {
    _acc.contents = f(i.contents)(_acc.contents)(xs.get_Current());
    null;
    i.contents = (i.contents + 1);
    null;
  };
  return _acc.contents;
});
Seq__FoldIndexedAux$Unit__Object___Unit__Object___ = (function(f,acc,xs)
{
  var i = {contents: 0};
  var _acc = {contents: acc};
  while (xs.MoveNext())
  {
    _acc.contents = f(i.contents)(_acc.contents)(xs.get_Current());
    null;
    i.contents = (i.contents + 1);
    null;
  };
  return _acc.contents;
});
Seq__FoldIndexedAux$Unit__String_Unit__String = (function(f,acc,xs)
{
  var i = {contents: 0};
  var _acc = {contents: acc};
  while (xs.MoveNext())
  {
    _acc.contents = f(i.contents)(_acc.contents)(xs.get_Current());
    null;
    i.contents = (i.contents + 1);
    null;
  };
  return _acc.contents;
});
Seq__FromFactory$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(f)
{
  var impl;
  impl = (new CreateEnumerable_1_FSharpAsync_1_Object_____ctor$FSharpAsync_1_Object___(f));
  return {GetEnumerator: (function(unitVar1)
  {
    return (function(__,unitVar1)
    {
      var _1454;
      return __.factory(_1454);
    })(impl, unitVar1);
  })};
});
Seq__FromFactory$Int32_Int32 = (function(f)
{
  var impl;
  impl = (new CreateEnumerable_1_Int32___ctor$Int32(f));
  return {GetEnumerator: (function(unitVar1)
  {
    return (function(__,unitVar1)
    {
      var _2268;
      return __.factory(_2268);
    })(impl, unitVar1);
  })};
});
Seq__FromFactory$Object___Object___ = (function(f)
{
  var impl;
  impl = (new CreateEnumerable_1_Object_____ctor$Object___(f));
  return {GetEnumerator: (function(unitVar1)
  {
    return (function(__,unitVar1)
    {
      var _2542;
      return __.factory(_2542);
    })(impl, unitVar1);
  })};
});
Seq__FromFactory$String_String = (function(f)
{
  var impl;
  impl = (new CreateEnumerable_1_String___ctor$String(f));
  return {GetEnumerator: (function(unitVar1)
  {
    return (function(__,unitVar1)
    {
      var _185;
      return __.factory(_185);
    })(impl, unitVar1);
  })};
});
Seq__IterateIndexed$Double_Double = (function(f,xs)
{
  var _3565;
  return Seq__FoldIndexed$Double__Unit_Double_Unit_((function(i)
  {
    return (function(unitVar1)
    {
      return (function(x)
      {
        return f(i)(x);
      });
    });
  }), _3565, xs);
});
Seq__IterateIndexed$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(f,xs)
{
  var _1485;
  return Seq__FoldIndexed$FSharpAsync_1_Object____Unit_FSharpAsync_1_Object____Unit_((function(i)
  {
    return (function(unitVar1)
    {
      return (function(x)
      {
        return f(i)(x);
      });
    });
  }), _1485, xs);
});
Seq__IterateIndexed$Object___Object___ = (function(f,xs)
{
  var _2568;
  return Seq__FoldIndexed$Object____Unit_Object____Unit_((function(i)
  {
    return (function(unitVar1)
    {
      return (function(x)
      {
        return f(i)(x);
      });
    });
  }), _2568, xs);
});
Seq__IterateIndexed$String_String = (function(f,xs)
{
  var _209;
  return Seq__FoldIndexed$String__Unit_String_Unit_((function(i)
  {
    return (function(unitVar1)
    {
      return (function(x)
      {
        return f(i)(x);
      });
    });
  }), _209, xs);
});
Seq__Map$Int32__FSharpAsync_1_Object___Int32_FSharpAsync_1_Object___ = (function(f,xs)
{
  return (function(_f)
  {
    return Seq__Delay$FSharpAsync_1_Object___FSharpAsync_1_Object___(_f);
  })((function(unitVar0)
  {
    var _f = (function(_enum)
    {
      if (_enum.MoveNext()) 
      {
        return {Tag: 1.000000, Value: (new TupleFSharpAsync_1_Object____IEnumerator_1_Int32_(f(_enum.get_Current()), _enum))};
      }
      else
      {
        return {Tag: 0.000000};
      };
    });
    return (function(seed)
    {
      return Seq__Unfold$IEnumerator_1_Int32__FSharpAsync_1_Object___IEnumerator_1_Int32__FSharpAsync_1_Object___(_f, seed);
    })(Seq__Enumerator$Int32_Int32(xs));
  }));
});
Seq__OfArray$Object___Object___ = (function(xs)
{
  var f = (function(i)
  {
    if ((i < Array__BoxedLength$(xs))) 
    {
      return {Tag: 1.000000, Value: (new TupleObject____Int32(xs[i], (i + 1)))};
    }
    else
    {
      return {Tag: 0.000000};
    };
  });
  return (function(seed)
  {
    return Seq__Unfold$Int32__Object___Int32_Object___(f, seed);
  })(0);
});
Seq__OfArray$String_String = (function(xs)
{
  var f = (function(i)
  {
    if ((i < Array__BoxedLength$(xs))) 
    {
      return {Tag: 1.000000, Value: (new TupleString_Int32(xs[i], (i + 1)))};
    }
    else
    {
      return {Tag: 0.000000};
    };
  });
  return (function(seed)
  {
    return Seq__Unfold$Int32__String_Int32_String(f, seed);
  })(0);
});
Seq__OfList$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  var f = (function(_arg1)
  {
    if ((_arg1.Tag == 1.000000)) 
    {
      var _xs = List__Tail$FSharpAsync_1_Object___FSharpAsync_1_Object___(_arg1);
      var x = List__Head$FSharpAsync_1_Object___FSharpAsync_1_Object___(_arg1);
      return {Tag: 1.000000, Value: (new TupleFSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___(x, _xs))};
    }
    else
    {
      return {Tag: 0.000000};
    };
  });
  return (function(seed)
  {
    return Seq__Unfold$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___(f, seed);
  })(xs);
});
Seq__OfList$String_String = (function(xs)
{
  var f = (function(_arg1)
  {
    if ((_arg1.Tag == 1.000000)) 
    {
      var _xs = List__Tail$String_String(_arg1);
      var x = List__Head$String_String(_arg1);
      return {Tag: 1.000000, Value: (new TupleString_FSharpList_1_String_(x, _xs))};
    }
    else
    {
      return {Tag: 0.000000};
    };
  });
  return (function(seed)
  {
    return Seq__Unfold$FSharpList_1_String__String_FSharpList_1_String__String(f, seed);
  })(xs);
});
Seq__ToArray$Double_Double = (function(xs)
{
  var ys = Array__ZeroCreate$Double_Double(0);
  var f = (function(i)
  {
    return (function(x)
    {
      ys[i] = x;
      return null;
    });
  });
  (function(_xs)
  {
    return Seq__IterateIndexed$Double_Double(f, _xs);
  })(xs);
  return ys;
});
Seq__ToArray$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  var ys = Array__ZeroCreate$FSharpAsync_1_Object___FSharpAsync_1_Object___(0);
  var f = (function(i)
  {
    return (function(x)
    {
      ys[i] = x;
      return null;
    });
  });
  (function(_xs)
  {
    return Seq__IterateIndexed$FSharpAsync_1_Object___FSharpAsync_1_Object___(f, _xs);
  })(xs);
  return ys;
});
Seq__ToArray$Object___Object___ = (function(xs)
{
  var ys = Array__ZeroCreate$Object___Object___(0);
  var f = (function(i)
  {
    return (function(x)
    {
      ys[i] = x;
      return null;
    });
  });
  (function(_xs)
  {
    return Seq__IterateIndexed$Object___Object___(f, _xs);
  })(xs);
  return ys;
});
Seq__ToArray$String_String = (function(xs)
{
  var ys = Array__ZeroCreate$String_String(0);
  var f = (function(i)
  {
    return (function(x)
    {
      ys[i] = x;
      return null;
    });
  });
  (function(_xs)
  {
    return Seq__IterateIndexed$String_String(f, _xs);
  })(xs);
  return ys;
});
Seq__ToList$FSharpAsync_1_Object___FSharpAsync_1_Object___ = (function(xs)
{
  return (function(list)
  {
    return List__Reverse$FSharpAsync_1_Object___FSharpAsync_1_Object___(list);
  })(Seq__Fold$FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___((function(acc)
  {
    return (function(x)
    {
      return List__CreateCons$FSharpAsync_1_Object___FSharpAsync_1_Object___(x, acc);
    });
  }), List__Empty$FSharpAsync_1_Object___FSharpAsync_1_Object___(), xs));
});
Seq__Unfold$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___ = (function(f,seed)
{
  return Seq__FromFactory$FSharpAsync_1_Object___FSharpAsync_1_Object___((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object_____ctor$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___Tuple_2_FSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$FSharpList_1_FSharpAsync_1_Object___FSharpList_1_FSharpAsync_1_Object___(__.acc) && (function()
        {
          var _1432;
          return next(_1432);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
Seq__Unfold$FSharpList_1_String__String_FSharpList_1_String__String = (function(f,seed)
{
  return Seq__FromFactory$String_String((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_FSharpList_1_String__String___ctor$FSharpList_1_String__String(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$FSharpList_1_String_FSharpList_1_String_(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_String__FSharpList_1_String_Tuple_2_String__FSharpList_1_String_(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_String__FSharpList_1_String_Tuple_2_String__FSharpList_1_String_(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$FSharpList_1_String_FSharpList_1_String_(__.acc) && (function()
        {
          var _163;
          return next(_163);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
Seq__Unfold$IEnumerator_1_Int32__FSharpAsync_1_Object___IEnumerator_1_Int32__FSharpAsync_1_Object___ = (function(f,seed)
{
  return Seq__FromFactory$FSharpAsync_1_Object___FSharpAsync_1_Object___((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_IEnumerator_1_Int32__FSharpAsync_1_Object_____ctor$IEnumerator_1_Int32__FSharpAsync_1_Object___(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$IEnumerator_1_Int32_IEnumerator_1_Int32_(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_Tuple_2_FSharpAsync_1_Object____IEnumerator_1_Int32_(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$IEnumerator_1_Int32_IEnumerator_1_Int32_(__.acc) && (function()
        {
          var _2330;
          return next(_2330);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
Seq__Unfold$Int32__Int32_Int32_Int32 = (function(f,seed)
{
  return Seq__FromFactory$Int32_Int32((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_Int32__Int32___ctor$Int32_Int32(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$Int32_Int32(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_Int32__Int32_Tuple_2_Int32__Int32_(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_Int32__Int32_Tuple_2_Int32__Int32_(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$Int32_Int32(__.acc) && (function()
        {
          var _2246;
          return next(_2246);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
Seq__Unfold$Int32__Object___Int32_Object___ = (function(f,seed)
{
  return Seq__FromFactory$Object___Object___((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_Int32__Object_____ctor$Int32_Object___(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$Int32_Int32(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_Object____Int32_Tuple_2_Object____Int32_(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_Object____Int32_Tuple_2_Object____Int32_(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$Int32_Int32(__.acc) && (function()
        {
          var _2520;
          return next(_2520);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
Seq__Unfold$Int32__String_Int32_String = (function(f,seed)
{
  return Seq__FromFactory$String_String((function(unitVar0)
  {
    var impl;
    impl = (new UnfoldEnumerator_2_Int32__String___ctor$Int32_String(seed, f));
    return {get_Current: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        return __.current;
      })(impl, unitVar1);
    }), Dispose: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        ;
      })(impl, unitVar1);
    }), MoveNext: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        var next = (function(_unitVar0)
        {
          var currAcc = Option__GetValue$Int32_Int32(__.acc);
          var x = __.unfold(currAcc);
          if ((x.Tag == 1.000000)) 
          {
            var value = Option__GetValue$Tuple_2_String__Int32_Tuple_2_String__Int32_(x).Items[0.000000];
            var nextAcc = Option__GetValue$Tuple_2_String__Int32_Tuple_2_String__Int32_(x).Items[1.000000];
            __.acc = {Tag: 1.000000, Value: nextAcc};
            __.current = value;
            return true;
          }
          else
          {
            __.acc = {Tag: 0.000000};
            __.current = null;
            return false;
          };
        });
        return (Option__IsSome$Int32_Int32(__.acc) && (function()
        {
          var _602;
          return next(_602);
        })());
      })(impl, unitVar1);
    }), Reset: (function(unitVar1)
    {
      return (function(__,unitVar1)
      {
        __.acc = {Tag: 1.000000, Value: __.seed};
        __.current = null;
      })(impl, unitVar1);
    })};
  }));
});
SetTreeModule__SetNode$String_String = (function(x,l,r,h)
{
  return (new SetTree_1_String__SetNodeString(x, l, r, h));
});
SetTreeModule__SetOne$String_String = (function(n)
{
  return (new SetTree_1_String__SetOneString(n));
});
SetTreeModule__add$String_String = (function(comparer,k,t)
{
  if ((t.Tag == 2.000000)) 
  {
    var k2 = t.Item;
    var c = comparer.Compare(k, k2);
    if ((c < 0)) 
    {
      return SetTreeModule__SetNode$String_String(k, (new SetTree_1_String__SetEmptyString()), t, 2);
    }
    else
    {
      if ((c == 0)) 
      {
        return t;
      }
      else
      {
        return SetTreeModule__SetNode$String_String(k, t, (new SetTree_1_String__SetEmptyString()), 2);
      };
    };
  }
  else
  {
    if ((t.Tag == 0.000000)) 
    {
      return SetTreeModule__SetOne$String_String(k);
    }
    else
    {
      var r = t.Item3;
      var l = t.Item2;
      var _k2 = t.Item1;
      var _c = comparer.Compare(k, _k2);
      if ((_c < 0)) 
      {
        return SetTreeModule__rebalance$String_String(SetTreeModule__add$String_String(comparer, k, l), _k2, r);
      }
      else
      {
        if ((_c == 0)) 
        {
          return t;
        }
        else
        {
          return SetTreeModule__rebalance$String_String(l, _k2, SetTreeModule__add$String_String(comparer, k, r));
        };
      };
    };
  };
});
SetTreeModule__get_tolerance$ = (function()
{
  return 2;
});
SetTreeModule__height$String_String = (function(t)
{
  if ((t.Tag == 2.000000)) 
  {
    return 1;
  }
  else
  {
    if ((t.Tag == 1.000000)) 
    {
      var h = t.Item4;
      return h;
    }
    else
    {
      return 0;
    };
  };
});
SetTreeModule__mem$IComparable_IComparable_ = (function(comparer,k,t)
{
  if ((t.Tag == 2.000000)) 
  {
    var k2 = t.Item;
    return (comparer.Compare(k, k2) == 0);
  }
  else
  {
    if ((t.Tag == 0.000000)) 
    {
      return false;
    }
    else
    {
      var r = t.Item3;
      var l = t.Item2;
      var _k2 = t.Item1;
      var c = comparer.Compare(k, _k2);
      if ((c < 0)) 
      {
        return SetTreeModule__mem$IComparable_IComparable_(comparer, k, l);
      }
      else
      {
        return ((c == 0) || SetTreeModule__mem$IComparable_IComparable_(comparer, k, r));
      };
    };
  };
});
SetTreeModule__mk$String_String = (function(l,k,r)
{
  var matchValue = (new TupleSetTree_1_String__SetTree_1_String_(l, r));
  if ((matchValue.Items[0.000000].Tag == 0.000000)) 
  {
    if ((matchValue.Items[1.000000].Tag == 0.000000)) 
    {
      return SetTreeModule__SetOne$String_String(k);
    }
    else
    {
      var hl = SetTreeModule__height$String_String(l);
      var hr = SetTreeModule__height$String_String(r);
      var _761;
      if ((hl < hr)) 
      {
        _761 = hr;
      }
      else
      {
        _761 = hl;
      };
      var m = _761;
      return SetTreeModule__SetNode$String_String(k, l, r, (m + 1));
    };
  }
  else
  {
    var _hl = SetTreeModule__height$String_String(l);
    var _hr = SetTreeModule__height$String_String(r);
    var _775;
    if ((_hl < _hr)) 
    {
      _775 = _hr;
    }
    else
    {
      _775 = _hl;
    };
    var _m = _775;
    return SetTreeModule__SetNode$String_String(k, l, r, (_m + 1));
  };
});
SetTreeModule__mkFromEnumerator$String_String = (function(comparer,acc,e)
{
  if (e.MoveNext()) 
  {
    return SetTreeModule__mkFromEnumerator$String_String(comparer, SetTreeModule__add$String_String(comparer, e.get_Current(), acc), e);
  }
  else
  {
    return acc;
  };
});
SetTreeModule__ofSeq$String_String = (function(comparer,c)
{
  var ie = c.GetEnumerator();
  try
  {
    return SetTreeModule__mkFromEnumerator$String_String(comparer, (new SetTree_1_String__SetEmptyString()), ie);
  }
  finally{
    if (false) 
    {
      LanguagePrimitives__UnboxGeneric$IDisposable_IDisposable_(ie).Dispose();
    }
    else
    {
      ;
    };
  };
});
SetTreeModule__rebalance$String_String = (function(t1,k,t2)
{
  var t1h = SetTreeModule__height$String_String(t1);
  var t2h = SetTreeModule__height$String_String(t2);
  if ((t2h > (t1h + SetTreeModule__tolerance))) 
  {
    if ((t2.Tag == 1.000000)) 
    {
      var t2r = t2.Item3;
      var t2l = t2.Item2;
      var t2k = t2.Item1;
      if ((SetTreeModule__height$String_String(t2l) > (t1h + 1))) 
      {
        if ((t2l.Tag == 1.000000)) 
        {
          var t2lr = t2l.Item3;
          var t2ll = t2l.Item2;
          var t2lk = t2l.Item1;
          return SetTreeModule__mk$String_String(SetTreeModule__mk$String_String(t1, k, t2ll), t2lk, SetTreeModule__mk$String_String(t2lr, t2k, t2r));
        }
        else
        {
          throw ("rebalance");
          return null;
        };
      }
      else
      {
        return SetTreeModule__mk$String_String(SetTreeModule__mk$String_String(t1, k, t2l), t2k, t2r);
      };
    }
    else
    {
      throw ("rebalance");
      return null;
    };
  }
  else
  {
    if ((t1h > (t2h + SetTreeModule__tolerance))) 
    {
      if ((t1.Tag == 1.000000)) 
      {
        var t1r = t1.Item3;
        var t1l = t1.Item2;
        var t1k = t1.Item1;
        if ((SetTreeModule__height$String_String(t1r) > (t2h + 1))) 
        {
          if ((t1r.Tag == 1.000000)) 
          {
            var t1rr = t1r.Item3;
            var t1rl = t1r.Item2;
            var t1rk = t1r.Item1;
            return SetTreeModule__mk$String_String(SetTreeModule__mk$String_String(t1l, t1k, t1rl), t1rk, SetTreeModule__mk$String_String(t1rr, k, t2));
          }
          else
          {
            throw ("rebalance");
            return null;
          };
        }
        else
        {
          return SetTreeModule__mk$String_String(t1l, t1k, SetTreeModule__mk$String_String(t1r, k, t2));
        };
      }
      else
      {
        throw ("rebalance");
        return null;
      };
    }
    else
    {
      return SetTreeModule__mk$String_String(t1, k, t2);
    };
  };
});
SetTree_1_String__SetEmptyString = (function()
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "SetEmpty";
});
SetTree_1_String__SetNodeString = (function(Item1,Item2,Item3,Item4)
{
  var __this = this;
  __this.Tag = 1.000000;
  __this._CaseName = "SetNode";
  __this.Item1 = Item1;
  __this.Item2 = Item2;
  __this.Item3 = Item3;
  __this.Item4 = Item4;
});
SetTree_1_String__SetOneString = (function(Item)
{
  var __this = this;
  __this.Tag = 2.000000;
  __this._CaseName = "SetOne";
  __this.Item = Item;
});
Set_1_IComparable__Contains$IComparable_ = (function(s,x)
{
  return SetTreeModule__mem$IComparable_IComparable_(Set_1_IComparable__get_Comparer$IComparable_(s), x, Set_1_IComparable__get_Tree$IComparable_(s));
});
Set_1_IComparable__get_Comparer$IComparable_ = (function(set,unitVar1)
{
  return set.comparer_479;
});
Set_1_IComparable__get_Tree$IComparable_ = (function(set,unitVar1)
{
  return set.tree_483;
});
Set_1_String__From$String = (function(elements)
{
  var comparer = (new GenericComparer_1_String___ctor$String());
  var impl;
  impl = comparer;
  var _impl;
  _impl = comparer;
  return (new Set_1_String___ctor$String({Compare: (function(x,y)
  {
    return (function(__,x,y)
    {
      return (x < y ? -1 : (x == y ? 0 : 1));
    })(impl, x, y);
  })}, SetTreeModule__ofSeq$String_String({Compare: (function(x,y)
  {
    return (function(__,x,y)
    {
      return (x < y ? -1 : (x == y ? 0 : 1));
    })(_impl, x, y);
  })}, elements)));
});
Set_1_String___ctor$String = (function(comparer,tree)
{
  var __this = this;
  __this.comparer_479 = comparer;
  __this.tree_483 = tree;
  __this.serializedData = null;
});
Set__OfSeq$String_String = (function(c)
{
  return Set_1_String__From$String(c);
});
TupleFSharpAsync_1_Object____FSharpList_1_FSharpAsync_1_Object___ = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleFSharpAsync_1_Object____IEnumerator_1_Int32_ = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleFSharpFunc_2_Object______Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_ = (function(Item0,Item1,Item2)
{
  var __this = this;
  __this.Items = [Item0, Item1, Item2];
  __this.Items = [Item0, Item1, Item2];
  __this.Items = [Item0, Item1, Item2];
});
TupleFSharpFunc_2_String__Unit__FSharpFunc_2_Exception__Unit__FSharpFunc_2_String__Unit_ = (function(Item0,Item1,Item2)
{
  var __this = this;
  __this.Items = [Item0, Item1, Item2];
  __this.Items = [Item0, Item1, Item2];
  __this.Items = [Item0, Item1, Item2];
});
TupleInt32_Int32 = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleObject____Int32 = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleSetTree_1_String__SetTree_1_String_ = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleString_Double = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleString_FSharpList_1_String_ = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleString_Int32 = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleString_Object___ = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
TupleString_String = (function(Item0,Item1)
{
  var __this = this;
  __this.Items = [Item0, Item1];
  __this.Items = [Item0, Item1];
});
UnfoldEnumerator_2_FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object_____ctor$FSharpList_1_FSharpAsync_1_Object____FSharpAsync_1_Object___ = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
UnfoldEnumerator_2_FSharpList_1_String__String___ctor$FSharpList_1_String__String = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
UnfoldEnumerator_2_IEnumerator_1_Int32__FSharpAsync_1_Object_____ctor$IEnumerator_1_Int32__FSharpAsync_1_Object___ = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
UnfoldEnumerator_2_Int32__Int32___ctor$Int32_Int32 = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
UnfoldEnumerator_2_Int32__Object_____ctor$Int32_Object___ = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
UnfoldEnumerator_2_Int32__String___ctor$Int32_String = (function(seed,unfold)
{
  var __this = this;
  __this.seed = seed;
  __this.unfold = unfold;
  __this.acc = {Tag: 1.000000, Value: __this.seed};
  __this.current = null;
});
WorldBank__getByYear$ = (function(year,id)
{
  return (function(builder_)
  {
    return AsyncBuilder__Delay$Tuple_2___Tuple_2___(builder_, (function(unitVar)
    {
      return AsyncBuilder__Bind$FSharpSet_1_String__Tuple_2___FSharpSet_1_String__Tuple_2___(builder_, WorldBank__getCountryIds$(), (function(_arg1)
      {
        var countries = _arg1;
        return AsyncBuilder__Bind$Object____Tuple_2___Object____Tuple_2___(builder_, WorldBank__worldBankDownloadAll$(List__CreateCons$String_String("countries", List__CreateCons$String_String("indicators", List__CreateCons$String_String(id, List__Empty$String_String()))), List__CreateCons$Tuple_2_String__String_Tuple_2_String__String_((new TupleString_String("date", ((year.year.toString() + ":") + year.year.toString()))), List__Empty$Tuple_2_String__String_Tuple_2_String__String_())), (function(_arg2)
        {
          var data = _arg2;
          var chooser = (function(v)
          {
            var _2676;
            if (Helpers__isNull$(Helpers__getProperty$Object_Object_(v, "value"))) 
            {
              _2676 = {Tag: 0.000000};
            }
            else
            {
              _2676 = {Tag: 1.000000, Value: (1.000000 * Helpers__getProperty$Object_Object_(v, "value"))};
            };
            if (Option__IsNone$Decimal_Decimal(_2676)) 
            {
              return {Tag: 0.000000};
            }
            else
            {
              var _this = Helpers__getProperty$Object_Object_(v, "country");
              if ((!Set_1_IComparable__Contains$IComparable_(countries, Helpers__getProperty$Object_Object_(_this, "id")))) 
              {
                return {Tag: 0.000000};
              }
              else
              {
                var __this = Helpers__getProperty$Object_Object_(v, "country");
                var _2772;
                if (Helpers__isNull$(Helpers__getProperty$Object_Object_(v, "value"))) 
                {
                  _2772 = {Tag: 0.000000};
                }
                else
                {
                  _2772 = {Tag: 1.000000, Value: (1.000000 * Helpers__getProperty$Object_Object_(v, "value"))};
                };
                return {Tag: 1.000000, Value: (new TupleString_Double(Helpers__getProperty$Object_Object_(__this, "value"), Option__GetValue$Decimal_Decimal(_2772)))};
              };
            };
          });
          return AsyncBuilder__Return$Tuple_2___Tuple_2___(builder_, (function(array)
          {
            return Array__Choose$Object__Tuple_2_String__Double_Object__Tuple_2_String__Double_(chooser, array);
          })(data));
        }));
      }));
    }));
  })(Async_1_get_async$());
});
WorldBank__getCountryIds$ = (function(unitVar0)
{
  return (function(builder_)
  {
    return AsyncBuilder__Delay$FSharpSet_1_String_FSharpSet_1_String_(builder_, (function(unitVar)
    {
      var key = "thegamma_worldbank_country_ids";
      var matchValue = Helpers__tryGetGlobal$Object_Object_(key);
      if ((matchValue.Tag == 0.000000)) 
      {
        return AsyncBuilder__Bind$String__FSharpSet_1_String_String_FSharpSet_1_String_(builder_, WorldBank__worldBankDownload$(List__CreateCons$String_String("country", List__Empty$String_String()), List__Empty$Tuple_2_String__String_Tuple_2_String__String_()), (function(_arg1)
        {
          var json = _arg1;
          var object = json;
          var data = object;
          var mapping = (function(a)
          {
            return Helpers__getProperty$Object_Object_(a, "iso2Code");
          });
          var predicate = (function(a)
          {
            var _this = Helpers__getProperty$Object_Object_(a, "region");
            return (Helpers__getProperty$Object_Object_(_this, "id") != "NA");
          });
          var _mapping = (function(x)
          {
            return x;
          });
          var res = (function(elements)
          {
            return Set__OfSeq$String_String(Seq__OfArray$String_String(elements));
          })((function(array)
          {
            return Array__Map$Object__String_Object__String(mapping, array);
          })((function(array)
          {
            return Array__Filter$Object_Object_(predicate, array);
          })((function(value)
          {
            return value;
          })((function(array)
          {
            return Array__Map$Object__Object_Object__Object_(_mapping, array);
          })(Json__getArrayMemberByTag$("Array", data))))));
          Helpers__setGlobal$FSharpSet_1_String_FSharpSet_1_String_(key, res);
          return AsyncBuilder__Return$FSharpSet_1_String_FSharpSet_1_String_(builder_, res);
        }));
      }
      else
      {
        var v = Option__GetValue$Object_Object_(matchValue);
        return AsyncBuilder__Return$FSharpSet_1_String_FSharpSet_1_String_(builder_, Helpers__getGlobal$FSharpSet_1_String_FSharpSet_1_String_(key));
      };
    }));
  })(Async_1_get_async$());
});
WorldBank__getYear$ = (function(year)
{
  return (new Year___ctor$(year));
});
WorldBank__worldBankDownload$ = (function(functions,props)
{
  return Async__FromContinuations$String_String((function(tupledArg)
  {
    var cont = tupledArg.Items[0.000000];
    var econt = tupledArg.Items[1.000000];
    var ccont = tupledArg.Items[2.000000];
    return Helpers__getJSONPrefix$(WorldBank__worldBankUrl$(functions, props), (function(json)
    {
      return cont(json);
    }));
  }));
});
WorldBank__worldBankDownloadAll$ = (function(functions,props)
{
  return (function(builder_)
  {
    return AsyncBuilder__Delay$Object___Object___(builder_, (function(unitVar)
    {
      return AsyncBuilder__Bind$String__Object___String_Object___(builder_, WorldBank__worldBankDownload$(functions, props), (function(_arg1)
      {
        var json = _arg1;
        var object = json;
        var first = object;
        return AsyncBuilder__Bind$Object______Object___Object______Object___(builder_, (function(work)
        {
          return Async_2_PseudoParallel$Object___Object___(Seq__OfList$FSharpAsync_1_Object___FSharpAsync_1_Object___(work));
        })(Seq__ToList$FSharpAsync_1_Object___FSharpAsync_1_Object___(Seq__Delay$FSharpAsync_1_Object___FSharpAsync_1_Object___((function(_unitVar)
        {
          var _this = Json__getArrayMemberByTag$("Record", first);
          return Seq__Map$Int32__FSharpAsync_1_Object___Int32_FSharpAsync_1_Object___((function(p)
          {
            return (function(_builder_)
            {
              return AsyncBuilder__Delay$Object___Object___(_builder_, (function(__unitVar)
              {
                return AsyncBuilder__Bind$String__Object___String_Object___(_builder_, WorldBank__worldBankDownload$(functions, List__CreateCons$Tuple_2_String__String_Tuple_2_String__String_((new TupleString_String("page", p.toString())), props)), (function(_arg2)
                {
                  var _json = _arg2;
                  var _object = _json;
                  var res = _object;
                  var mapping = (function(x)
                  {
                    return x;
                  });
                  return AsyncBuilder__Return$Object___Object___(_builder_, (function(value)
                  {
                    return value;
                  })((function(array)
                  {
                    return Array__Map$Object__Object_Object__Object_(mapping, array);
                  })(Json__getArrayMemberByTag$("Array", res))));
                }));
              }));
            })(Async_1_get_async$());
          }), Range__oneStep$Int32_Int32(2, (1 * Helpers__getProperty$Object_Object_(_this, "pages"))));
        })))), (function(_arg3)
        {
          var alldata = _arg3;
          var mapping = (function(x)
          {
            return x;
          });
          return AsyncBuilder__Return$Object___Object___(builder_, (function(arrays)
          {
            return Array__Concat$Object_Object_(Seq__OfArray$Object___Object___(arrays));
          })(Array__Append$Object___Object___([(function(value)
          {
            return value;
          })((function(array)
          {
            return Array__Map$Object__Object_Object__Object_(mapping, array);
          })(Json__getArrayMemberByTag$("Array", first)))], alldata)));
        }));
      }));
    }));
  })(Async_1_get_async$());
});
WorldBank__worldBankUrl$ = (function(functions,props)
{
  var sep = "";
  var mapping = (function(m)
  {
    return ("/" + Helpers__encodeURIComponent$(m));
  });
  var _sep = "";
  var _mapping = (function(tupledArg)
  {
    var key = tupledArg.Items[0.000000];
    var value = tupledArg.Items[1.000000];
    return ((("\u0026" + key) + "=") + Helpers__encodeURIComponent$(value));
  });
  return ((("http://api.worldbank.org/" + (function(strings)
  {
    return FSharpString__Concat$(sep, Seq__OfList$String_String(strings));
  })((function(list)
  {
    return List__Map$String__String_String_String(mapping, list);
  })(functions))) + "?per_page=1000\u0026format=jsonp") + (function(strings)
  {
    return FSharpString__Concat$(_sep, Seq__OfList$String_String(strings));
  })((function(list)
  {
    return List__Map$Tuple_2_String__String__String_Tuple_2_String__String__String(_mapping, list);
  })(props)));
});
Year___ctor$ = (function(year)
{
  var __this = this;
  __this.year = year;
});
chart__geo$ = (function(data)
{
  return (new Geo___ctor$(ChartDataModule__oneKeyValue$String_String("string", data), "GeoChart", Extensions__GeoChartOptions_get_empty_Static$()));
});
list_1_FSharpAsync_1_Object____ConsFSharpAsync_1_Object___ = (function(Item1,Item2)
{
  var __this = this;
  __this.Tag = 1.000000;
  __this._CaseName = "Cons";
  __this.Item1 = Item1;
  __this.Item2 = Item2;
});
list_1_FSharpAsync_1_Object____NilFSharpAsync_1_Object___ = (function()
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Nil";
});
list_1_String__ConsString = (function(Item1,Item2)
{
  var __this = this;
  __this.Tag = 1.000000;
  __this._CaseName = "Cons";
  __this.Item1 = Item1;
  __this.Item2 = Item2;
});
list_1_String__NilString = (function()
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Nil";
});
list_1_Tuple_2_String__String__ConsTuple_2_String__String_ = (function(Item1,Item2)
{
  var __this = this;
  __this.Tag = 1.000000;
  __this._CaseName = "Cons";
  __this.Item1 = Item1;
  __this.Item2 = Item2;
});
list_1_Tuple_2_String__String__NilTuple_2_String__String_ = (function()
{
  var __this = this;
  __this.Tag = 0.000000;
  __this._CaseName = "Nil";
});
series_2_String__Double___ctor$String_Double = (function(data,keyName,valueName,seriesName)
{
  var __this = this;
  __this.data = data;
  __this.keyName = keyName;
  __this.valueName = valueName;
  __this.seriesName = seriesName;
});
series_2_String__Double__set$String__Object___String_Double_String_Object___ = (function(x,data,keyName,valueName,seriesName)
{
  var _3267;
  if ((keyName.Tag == 1.000000)) 
  {
    var v = Option__GetValue$String_String(keyName);
    _3267 = v;
  }
  else
  {
    _3267 = x.keyName;
  };
  var _3275;
  if ((valueName.Tag == 1.000000)) 
  {
    var v = Option__GetValue$String_String(valueName);
    _3275 = v;
  }
  else
  {
    _3275 = x.valueName;
  };
  var _3283;
  if ((seriesName.Tag == 1.000000)) 
  {
    var v = Option__GetValue$String_String(seriesName);
    _3283 = v;
  }
  else
  {
    _3283 = x.seriesName;
  };
  return (new series_2_String__Object_____ctor$String_Object___(data, _3267, _3275, _3283));
});
series_2_String__Object_____ctor$String_Object___ = (function(data,keyName,valueName,seriesName)
{
  var __this = this;
  __this.data = data;
  __this.keyName = keyName;
  __this.valueName = valueName;
  __this.seriesName = seriesName;
});
series__create$String__Double_String_Double = (function(data,keyName,valueName,seriesName)
{
  return (new series_2_String__Double___ctor$String_Double(data, keyName, valueName, seriesName));
});
SetTreeModule__tolerance = SetTreeModule__get_tolerance$();
var colorScale = List__CreateCons$String_String("#6CC627", List__CreateCons$String_String("#DB9B3B", List__CreateCons$String_String("#DB7532", List__CreateCons$String_String("#DD5321", List__CreateCons$String_String("#DB321C", List__CreateCons$String_String("#E00B00", List__Empty$String_String()))))));
var _31;
var _this = _31;
var __this = WorldBank__getYear$(2010);
var ___this = __this;
var co2 = series__create$String__Double_String_Double(WorldBank__getByYear$(___this, "EN.ATM.CO2E.KT"), "Country", "Value", "CO2 emissions (kt)");
return Geo__show$(Geo__colorAxis$(chart__geo$(co2), {Tag: 0.000000}, {Tag: 0.000000}, {Tag: 0.000000}, {Tag: 1.000000, Value: Seq__OfList$String_String(colorScale)}))
}); //();