using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers {
	public class HomeController : Controller
	{
		private readonly FamilySysDbContext db;

		public HomeController(FamilySysDbContext _db)
		{
			db = _db;
		}

		public IActionResult Index()
		{
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			}
			else
			{
				return View();
			}
		}

		public IActionResult Error()
		{
			return View();
		}

		public IActionResult nonMemberAlarm()
		{
			return View();
		}

		public IActionResult Members()
		{
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			}
			else
			{
				try
				{
					var Users = db.Users.Where(x => x.IsAdmin == 0);
					return View(Users);
				}
				catch (Exception)
				{
					return RedirectToAction("Error");
				}
			}
		}
	}
}
