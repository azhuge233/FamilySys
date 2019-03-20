using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Models.ViewModels.AdminViewModel;
using FamilySys.Models.ViewModels.MemberViewModel;
using FamilySys.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
	public class AdminController : Controller {
		private readonly Encryption encryption;
		private readonly FamilySysDbContext db;

		public AdminController(FamilySysDbContext _db, Encryption _encryption) {
			db = _db;
			encryption = _encryption;
		}

		public JsonResult UsernameValidationJsonResult(string id, string username) {
			if (db.Users.Single(x => x.ID == id).Username == username)
				return Json(true);
			bool isExists = db.Users.Any(x => x.Username == username);
			return Json(!isExists);
		}

		public string GetRandomNum() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString();
			} while (db.Announcements.Any(x => x.ID == ID));

			return ID;
		}

		public IActionResult Index() {
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
				return RedirectToAction("Error", "Member");
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
				var me = db.Users.Single(x => x.IsAdmin == 1);

				var form = new Member_MyInfo_ChgPwd_ViewModel() {
					ID = me.ID,
					Username = me.Username,
					Sex = me.Sex,
					Phone = me.Phone,
					Mail = me.Mail,
					Score = me.Score
				};

				return View(form);

			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("MyInfo", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult ChgInfo(Member_MyInfo_ChgPwd_ViewModel form) {
			try {
				var user = db.Users.Single(x => x.IsAdmin == 1);

				user.Sex = form.Sex;
				user.Phone = form.Phone;
				user.Mail = form.Mail;

				db.SaveChanges();

				return RedirectToAction("MyInfo");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		[HttpPost]
		public IActionResult ChgPwd(Member_MyInfo_ChgPwd_ViewModel form) {
			try {
				if (encryption.Encrypt(form.Password) == db.Users.Single(x => x.IsAdmin == 1).Password) {
					var user = db.Users.Single(x => x.ID == form.ID);

					user.Password = encryption.Encrypt(form.NewPassword);
					db.SaveChanges();

					TempData["msg"] = "<script>alert('密码已修改，请重新登录');</script>";
					return RedirectToAction("Logout");
				} else {
					TempData["ErrMsg"] = "原密码输入错误";
					return RedirectToAction("MyInfo");
				}
			} catch(Exception ex) {
				TempData["ErrMsg"] = "alert(\'" + ex.Message.ToString() + "\');";
				return RedirectToAction("Error");
			}
		}

		public IActionResult Members() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0);
					return View(Users);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}

			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Members", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult EditMemberInfo(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var user = db.Users.Single(x => x.ID == ID);

					var userViewModel = new AdminEditMemberViewModel() {
						ID = user.ID,
						Username = user.Username,
						Password = "",
						Sex = user.Sex,
						Phone = user.Phone,
						Mail = user.Mail,
						Score = user.Score
					};

					return View(userViewModel);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoEditMemberInfo(AdminEditMemberViewModel form) {
			try {
				var user = db.Users.Single(x => x.ID == form.ID);

				user.Username = form.Username;
				user.Password = string.IsNullOrEmpty(form.Password) ? user.Password : encryption.Encrypt(form.Password);
				user.Sex = form.Sex;
				user.Mail = form.Mail;
				user.Phone = form.Phone;
				user.Score = form.Score;

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'用户 " + user.Username + " 的信息已修改\');</script>";
				return RedirectToAction("Members");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowAnnouncements() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var Annos = db.Announcements.Where(x => x.ID == x.ID).OrderByDescending(x => x.Date);
					return View(Annos);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowAnnouncements", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult PublishAnnouncement() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return View();
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoPublishAnnouncement(AnnouncementViewModel form) {
			try {
				var newAnno = new Announcement() {
					ID = GetRandomNum(),
					Date = DateTime.Now,
					ModifyDate = DateTime.Now,
					Title = form.Title,
					Content = form.Content.Replace("\n", "<br />")
				};

				db.Announcements.Add(newAnno);
				db.SaveChanges();
				
				TempData["Success"] = "<script>alert(\'公告 " + newAnno.Title + " 发布成功\');</script>";
				return RedirectToAction("ShowAnnouncements");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowAnnoDetails(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				var Anno = db.Announcements.Single(x => x.ID == ID);
				return View(Anno);
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowAnnoDetails", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DelAnno(string ID) {
			try {
				var delAnno = db.Announcements.Single(x => x.ID == ID);
				var title = delAnno.Title;

				db.Announcements.Remove(delAnno);
				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'公告 " + title + " 删除成功\');</script>";
				return RedirectToAction("ShowAnnouncements");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult EditAnno(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				var Anno = db.Announcements.Single(x => x.ID == ID);

				var AnnoViewModel = new AnnouncementViewModel() {
					ID = Anno.ID,
					Title = Anno.Title,
					Content = Anno.Content
				};

				return View(AnnoViewModel);
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoEditAnno(AnnouncementViewModel form) {
			try {
				var thisAnno = db.Announcements.Single(x => x.ID == form.ID);
				thisAnno.Title = form.Title;
				thisAnno.Content = form.Content;
				thisAnno.ModifyDate = DateTime.Now;

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'公告 " + thisAnno.Title + " 修改成功\');</script>";
				return RedirectToAction("ShowAnnouncements");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}
	}
}