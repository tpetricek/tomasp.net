// Page behaviour shared by every long read. The figure carousel is driven by inline
// onclick/onmouseover attributes the LaTeX converter emits, so these must stay global.

function moveNotes() {
  // Change hrefs of notes in the doc to point to sidenote/endnote depending on width
  var ref = window.innerWidth < 970 ? "endnote" : "note";
  for(var a of document.querySelectorAll(".noteref a"))
  a.href = "#" + ref + "_" + a.innerText;

  // If footnotes on the margin overlap, move them a bit to make them fit nicely
  var lasty = 0;
  for(var n of document.querySelectorAll("span.note")) 
    n.style = "margin-top: 0px";
  for(var n of document.querySelectorAll("span.note")) {
    var rect = n.getBoundingClientRect();
    var nowy = rect.y + window.scrollY;
    if (nowy < lasty) n.style = "margin-top:" + Math.round(lasty - nowy) + "px";
    rect = n.getBoundingClientRect();
    lasty = rect.y + window.scrollY + rect.height + 20;
  }
}

function switchFigure(prefix, index) {
  document.getElementById(prefix + 'scroll').style.marginLeft="-" + (index*100) + "%";
  var links = document.querySelectorAll("#" + prefix + "previews a");
  for(var i = 0; i < links.length; i++) links[i].className = i == index ? "selected" : "";
}

function startScroll(prefix,offs,once) {
  let f = once ? window.setTimeout : window.setInterval;
  window.clearInterval(window.figIntervalId);
  window.figIntervalId = f(() => {
    document.getElementById(prefix+"previews").scrollBy({left:offs, top:0, behavior:'smooth'});
  }, 100);    
}
function endScroll() {
  window.clearInterval(window.figIntervalId);
}

window.addEventListener('load', moveNotes);
window.addEventListener('resize', moveNotes);
window.addEventListener('load', hljs.highlightAll);
