using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return View();
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

        public IActionResult Error() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return View();
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

        public IActionResult Logout() {
	        HttpContext.Session.Clear();
	        return RedirectToAction("Index", "Home");
        }
	}
}