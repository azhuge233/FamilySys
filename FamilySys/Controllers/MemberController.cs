using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
	        if (HttpContext.Session.GetInt32("isAdmin") == 1) {
		        return RedirectToAction("Index", "Admin");
	        } else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
		        return View();
	        }
	        else
	        {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
        }

        public IActionResult Error()
        {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return View();
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}
    }
}