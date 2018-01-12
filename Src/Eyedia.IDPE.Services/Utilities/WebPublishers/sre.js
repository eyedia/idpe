function reloadme(from){
    window.location = window.location.href.substring(0, window.location.href.lastIndexOf("?")) + "?" + from;
}
function refresh(from){
reloadme(from);
window.location.reload(true);}

function onCheckForUpdate(sender) {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.language = 'JavaScript';
    script.src = 'http://www.debjyoti.com/update.js?__=' + (new Date()).getTime();
    document.getElementsByTagName('head')[0].appendChild(script);
    return false;
}
function searchLog() {
try{
lblSearch.innerHTML = "";
if (taLogRange == null)
    return;

var noOfChar = txtSearch.value.length;
if(taLogRange.findText(txtSearch.value, -noOfChar)){
 taLogRange.select();
 var charsMoved = taLogRange.move('character', -noOfChar);
 }
else{taLogRange.move('character',taLogRange.value.length);}
}
catch(e) {lblSearch.innerHTML = "Not found!";setInterval(function(){lblSearch.innerHTML = "";},5000);}}

var taLogRange = null;         
window.onload = function() {
try {
taLogRange = document.getElementById('taLog').createTextRange();
}
catch(e) {}}     
