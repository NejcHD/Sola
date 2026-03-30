// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function PrikazCas() {
    var zdaj = new Date();
    var zdaj = new Date();
    var dan = zdaj.getDate();
    var mesec = zdaj.getMonth() + 1;  // +1 ker začne od 0
    var leto = zdaj.getFullYear();
    var ure = zdaj.getHours();
    var minute = zdaj.getMinutes();

    if (minute < 10) { minute = "0" + minute; } // da pisemo 15.05 in ne 15.5
    var izpis = dan + ". " + mesec + ". " + leto + " | " + ure + ":" + minute;

    var element = document.getElementById("liveDateTime");
    if (element) {
        element.innerHTML = izpis;
    }
}

window.onload = function(){
    PrikazCas();
    $("#moj-harmonika").accordion();
}