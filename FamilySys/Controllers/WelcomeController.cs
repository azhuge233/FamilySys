using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Models.ViewModels.WelcomeViewModel;
using FamilySys.Modules;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers {
	public class WelcomeController : Controller {
		private readonly FamilySysDbContext db;
		private readonly Encryption encryption;

		public WelcomeController(FamilySysDbContext _db, Encryption _encryption) {
			db = _db;
			encryption = _encryption;
		}

		public IActionResult Index() {
			if (db.Users.Any(x => x.Username == "admin")) {
				return RedirectToAction("Index", "Home");
			} else {
				var admin = new FirstRunConfigViewModel();
				
				return View(admin);
			}
		}

		[HttpPost]
		public IActionResult FirstRunConfig(FirstRunConfigViewModel form) {
			try {
				var admin = new User() {
					ID = form.ID,
					Username = form.Username,
					Password = encryption.Encrypt(form.Password),
					Sex = form.Sex,
					Phone = form.Phone,
					Mail = form.Mail,
					Score = 100,
					IsAdmin = 1
				};

				db.Users.Add(admin);
				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'系统初始化成功，跳转至主页\')</script>";
				return RedirectToAction("Index", "Home");
			} catch (Exception) {
				TempData["ErrMsg"] = "<script>alert(\'系统初始化失败，请重试\')</script>";
				return RedirectToAction("Index");
			}
		}
	}
}