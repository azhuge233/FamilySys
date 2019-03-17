// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ensure() {
	if(window.confirm('确认提交？')) {
		return true;
	} else {
		return false;
	}
}

function getCookie(name) {
	var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
	if (arr = document.cookie.match(reg))
		return arr[2];
	else
		return null;
}

function setCookie(name, value) {
	var Days = 1;
	var exp = new Date();
	exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
	document.cookie = name + "=" + value + ";expires=" + exp.toGMTString() + ";path=/";
}

function changeColor(btn, body, mode) {
	if (mode == "night") {
		btn.style.setProperty("background-color", "#000");
		btn.style.setProperty("color", "#fff");
		btn.innerHTML = "夜间";

		body.style.setProperty("background-color", "#333");
		body.style.setProperty("color", "#F5F5F5");
	} else {
		btn.style.setProperty("background-color", "#d2691e");
		btn.style.setProperty("color", "#e0ffff");
		btn.innerHTML = "日间";

		body.style.setProperty("background-color", "#fff");
		body.style.setProperty("color", "#333");
	}
}

function changeMode(thisID) {
	var btn = document.getElementById(thisID);
	var body = document.body;
	if (btn.innerHTML == "日间") {
		changeColor(btn, body, "night");
		setCookie("mode", "night");
	} else {
		changeColor(btn, body, "day");
		setCookie("mode", "sun");
	}
}

window.onload = function() {
};

function EnableTextBoxStats(usernameID, sexID, phoneID, mailID, cfmbtnID, rstbtnID, thisbtnID) {
	var thisBtn = document.getElementById(thisbtnID);
	var Cfmbtn = document.getElementById(cfmbtnID);
	var Rstbtn = document.getElementById(rstbtnID);
	var usernameTxb = document.getElementById(usernameID);
	var sexTxb = document.getElementById(sexID);
	var phoneTxb = document.getElementById(phoneID);
	var mailTxb = document.getElementById(mailID);

	thisBtn.type = "hidden";
	Cfmbtn.type = "submit";
	Rstbtn.type = "button";
	usernameTxb.disabled = false;
	sexTxb.disabled = false;
	phoneTxb.disabled = false;
	mailTxb.disabled = false;
}

function DisableTextBoxStats(usernameID, sexID, phoneID, mailID, subbtnID, cfmbtnID, thisbtnID) {
	var thisBtn = document.getElementById(thisbtnID);
	var Cfmbtn = document.getElementById(cfmbtnID);
	var Subbtn = document.getElementById(subbtnID);
	var usernameTxb = document.getElementById(usernameID);
	var sexTxb = document.getElementById(sexID);
	var phoneTxb = document.getElementById(phoneID);
	var mailTxb = document.getElementById(mailID);

	thisBtn.type = "hidden";
	Cfmbtn.type = "button";
	Subbtn.type = "hidden";
	usernameTxb.disabled = true;
	sexTxb.disabled = true;
	phoneTxb.disabled = true;
	mailTxb.disabled = true;
}