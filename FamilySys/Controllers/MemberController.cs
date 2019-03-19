﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Models.ViewModels;
using FamilySys.Models.ViewModels.MemberViewModel;
using FamilySys.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Rewrite.Internal;

namespace FamilySys.Controllers {
	public class MemberController : Controller {
		private readonly Encryption encryption;
		private readonly FamilySysDbContext db;

		public MemberController(FamilySysDbContext _db, Encryption _encryption) {
			db = _db;
			encryption = _encryption;
		}

		public JsonResult UsernameValidationJsonResult(string username) {
			if (username == HttpContext.Session.GetString("Username")) {
				return Json(true);
			}

			bool isExists = db.Users.Any(x => x.Username == username);
			return Json(!isExists);
		}

		public void CommonWork() {
			ViewBag.Username = HttpContext.Session.GetString("Username");
		}

		public string GetRandomNum() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString() + rd.Next(10000, 99999).ToString();
			} while (db.Houseworks.Any(x => x.ID == ID));

			return ID;
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
			return RedirectToAction("Index", "Login");
		}

		public IActionResult MyInfo() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("MyInfo", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				var me = db.Users.Single(x => x.ID == HttpContext.Session.GetString("ID"));

				var form = new Member_MyInfo_ChgPwd_ViewModel() {
						ID = me.ID,
						Username = me.Username,
						Sex = me.Sex,
						Phone = me.Phone,
						Mail = me.Mail
				};

				return View(form);
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult ChgInfo(Member_MyInfo_ChgPwd_ViewModel form) {
			try {
				var user = db.Users.Single(x => x.ID == HttpContext.Session.GetString("ID"));

				user.Username = form.Username;
				user.Sex = form.Sex;
				user.Phone = form.Phone;
				user.Mail = form.Mail;

				db.SaveChanges();

				return RedirectToAction("MyInfo");
			}
			catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		[HttpPost]
		public IActionResult ChgPwd(Member_MyInfo_ChgPwd_ViewModel form) {
			try {
				if (encryption.Encrypt(form.Password) == db.Users.Single(x => x.ID == form.ID).Password) {
					var user = db.Users.Single(x => x.ID == form.ID);

					user.Password = encryption.Encrypt(form.NewPassword);
					db.SaveChanges();

					TempData["msg"] = "<script>alert('密码已修改，请重新登录');</script>";
					return RedirectToAction("Logout");
				} else {
					TempData["ErrMsg"] = "原密码输入错误";
					return RedirectToAction("MyInfo");
				}
			}
			catch {
				return RedirectToAction("Error");
			}
		}

		public IActionResult Members() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Members", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0);
					return View(Users);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult ShowAnnouncements() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("ShowAnnouncements", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Annos = db.Announcements.Where(x => x.ID == x.ID).OrderByDescending(x => x.Date);
					return View(Annos);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult ShowAnnoDetails(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("ShowAnnoDetails", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Anno = db.Announcements.Single(x => x.ID == ID);
					return View(Anno);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult ShowHouseworks() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var houseworks = db.Houseworks.Select(x => x);
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
								ToUsername = housework.ToID == null ? "无" : db.Users.Single(x => x.ID == housework.ToID).Username
							}
						);
					}

					return View(houseworkShowcase.AsQueryable());
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult PublishHousework() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				var newHousework = new HouseworkPublishViewModel() {
					ID = GetRandomNum()
				};

				return View(newHousework);
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoPublishHousework(HouseworkPublishViewModel form) {
			try {
				var me = db.Users.Single(x => x.ID == HttpContext.Session.GetString("ID"));
				var newHousework = new Housework() {
					ID = form.ID,
					Title = form.Title,
					Content = form.Content,
					Type = form.Type,
					Score = form.Score,
					FromID = HttpContext.Session.GetString("ID"),
					Date = DateTime.Now,
					ModifyDate = DateTime.Now,
					IsDone = false
				};

				if (me.Score <= 0) {
					TempData["ErrMsg"] = "<script>alert(\'您的分值小于0，无法发布个人事务\')</script>";
					return RedirectToAction("ShowHouseworks");
				} else if (me.Score - form.Score < 0) {
					TempData["CannotPublish"] = "<script>alert(\'您的分值小于事务分值，无法发布事务 #" + form.ID + "\')</script>";
					return RedirectToAction("PublishHousework");
				} else {
					me.Score -= form.Type == 2 ? form.Score : 0;
				}

				db.Houseworks.Add(newHousework);
				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'事务 #" + newHousework.ID + " 已成功发布\')</script>";
				return RedirectToAction("ShowHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowHouseworkDetails(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				var housework = db.Houseworks.Single(x => x.ID == ID);
				var houseworkShowcase = new HouseworkShowcaseViewModel() {
					ID = housework.ID,
					Title = housework.Title,
					Content = housework.Content.Replace("\n", "<br />"),
					Score = housework.Score,
					Type = housework.Type,
					IsDone = housework.IsDone,
					Date = housework.Date,
					ModifyDate = housework.ModifyDate,
					FromID = housework.FromID,
					ToID = housework.ToID,
					FromUsername = db.Users.Single(x => x.ID == housework.FromID).Username,
					ToUsername = housework.ToID == null ? "无" : db.Users.Single(x => x.ID == housework.ToID).Username
				};

				return View(houseworkShowcase);
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}
	}
}