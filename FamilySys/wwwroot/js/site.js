// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getCookie(name) {
    var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
    if(arr=document.cookie.match(reg))
        return arr[2];
    else 
        return null;
}

function setCookie(name, value) {
    var Days = 1;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + value + ";expires=" + exp.toGMTString();
}

function changeColor(btn, mode) {
    if (mode == "night") {
        btn.style.setProperty('background-color', '#000');
        btn.style.setProperty('color', '#fff');
        btn.innerHTML = "夜间";
    } else {
        btn.style.setProperty('background-color', '#d2691e');
        btn.style.setProperty('color', '#e0ffff');
        btn.innerHTML = "日间";
    }
}

function changeMode(thisID) {
    var btn = document.getElementById(thisID);
    if (btn.innerHTML == "日间") {
        changeColor(btn, "night");
        
        setCookie('mode', 'night');
    } else {
        changeColor(btn, "day");
        
        setCookie('mode', 'sun');
    }

}

window.onload = function() {
    var btn = document.getElementById('chg-mode');
    var mode = getCookie("mode");
    changeColor(btn, mode);
}
