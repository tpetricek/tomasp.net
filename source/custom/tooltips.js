var currentTip = null;
var currentTipElement = null;

function hideTip(evt, name, unique)
{
  var el = document.getElementById(name);
  el.style.display = "none";
  currentTip = null;
}

function findPos(obj)
{
  var curleft = 0;
  var curtop = obj.offsetHeight;
  while (obj)
  {
    curleft += obj.offsetLeft;
    curtop += obj.offsetTop;
    obj = obj.offsetParent;
  };
  return [curleft, curtop];
}

function hideUsingEsc(e)
{
  if (!e) { e = event; }
  hideTip(e, currentTipElement, currentTip);
}

function showTip(evt, name, unique, owner)
{
  document.onkeydown = hideUsingEsc;
  if (currentTip == unique) return;
  currentTip = unique;
  currentTipElement = name;

  var pos = findPos(owner ? owner : (evt.srcElement ? evt.srcElement : evt.target));
  var posx = pos[0] + 0;
  var posy = pos[1] + 45;

  var el = document.getElementById(name);
  // The 'popover' attribute brings UA styling (position:fixed, inset:0, margin:auto,
  // its own border and colours) that fights the positioning below.
  el.removeAttribute("popover");
  var parent = (document.documentElement == null) ? document.body : document.documentElement;
  parent.appendChild(el);
  el.style.position = "absolute";
  el.style.left = posx + "px";
  el.style.top = posy + "px";
  el.style.display = "block";
}

// Wire up tips marked with 'data-fsdocs-tip' (older posts use inline handlers instead).
// Bound per element rather than delegated on 'document': 'mouseenter'/'mouseleave' do not
// bubble, so moving the mouse outside a snippet costs nothing.
document.addEventListener("DOMContentLoaded", function () {
  var tips = document.querySelectorAll("[data-fsdocs-tip]");
  for (var i = 0; i < tips.length; i++) {
    (function (el) {
      var name = el.getAttribute("data-fsdocs-tip");
      var unique = el.getAttribute("data-fsdocs-tip-unique");
      el.addEventListener("mouseenter", function (e) { showTip(e, name, unique, el); });
      el.addEventListener("mouseleave", function (e) { hideTip(e, name, unique); });
    })(tips[i]);
  }
});
