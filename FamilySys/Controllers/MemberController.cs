using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers {
	public class MemberController : Controller {
		private readonly FamilySysDbContext db;

		public MemberController(FamilySysDbContext _db) {
			db = _db;
		}

		public void CommonWork() {
			ViewBag.Username = HttpContext.Session.GetString("Username");
		}

		public IActionResult Index() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				return View();
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult Error() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				return View();
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult Logout() {
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult MyInfo() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				return View();
			} else {
				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult Members() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0);
					return View(Users);
				} catch (Exception) {
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("Index", "Home");
			}
		}
	}
}