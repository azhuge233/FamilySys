using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels;
using FamilySys.Models.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore;

namespace FamilySys.Controllers {
	public class HomeController : Controller
	{
		private readonly FamilySysDbContext db;

		public HomeController(FamilySysDbContext _db)
		{
			db = _db;
		}

		public virtual IActionResult Index() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1)
				return RedirectToAction("Index", "Admin");
			else if (HttpContext.Session.GetInt32("isAdmin") == 0)
				return RedirectToAction("Index", "Member");
			else {
				if (!db.Users.Any(x => x.Username == "admin")) {
					return RedirectToAction("Index", "Welcome");
				} else {
					return View();
				}
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

		[HttpGet]
		public IActionResult Members(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0).OrderBy(x => x.ID);

					var UsersPagedList = Users.ToPagedList(8, page);

					return View(UsersPagedList);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			}
		}

		[HttpGet]
		public IActionResult ShowHouseworks(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowHouseworks", "Member");
			} else {
				try {
					var houseworks = db.Houseworks.Select(x => x).OrderByDescending(x => x.Date);
					var houseworkShowcase = new List<HouseworkShowcaseViewModel>();

					foreach (var housework in houseworks) {
						houseworkShowcase.Add(
							new HouseworkShowcaseViewModel() {
								ID = housework.ID,
								Title = housework.Title,
								Content = housework.Content,
								Score = housework.Score,
								Type = housework.Type,
								IsDone = housework.IsDone,
								Date = housework.Date,
								ModifyDate = housework.ModifyDate,
								FromID = housework.FromID,
								ToID = housework.ToID,
								FromUsername = db.Users.Single(x => x.ID == housework.FromID).Username,
								ToUsername = housework.ToID == null ? "无" : db.Users.Single(x => x.ID == housework.ToID).Username,
								IsRated = db.Rates.Any(x => x.HouseworkID == housework.ID)
							}
						);
					}

					var houseworksPagedList = houseworkShowcase.ToPagedList(8, page);

					return View(houseworksPagedList);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			}
		}
	}
}
