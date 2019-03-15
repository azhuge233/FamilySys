using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels.MemberViewModel;
using FamilySys.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
    public class AdminController : Controller
    {
	    private readonly Encryption encryption;
	    private readonly FamilySysDbContext db;

	    public AdminController(FamilySysDbContext _db, Encryption _encryption) {
		    db = _db;
		    encryption = _encryption;
	    }

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

        public IActionResult MyInfo() {
	        if (HttpContext.Session.GetInt32("isAdmin") == 1) {
		        var me = db.Users.Single(x => x.IsAdmin == 1);

		        var form = new Member_MyInfo_ChgPwd_ViewModel() {
			        ID = me.ID,
			        Username = me.Username,
			        Sex = me.Sex,
			        Phone = me.Phone,
			        Mail = me.Mail
		        };

		        return View(form);
				
	        } else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
		        return RedirectToAction("Index", "Member");
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
	        } catch (Exception) {
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
	        } catch {
		        return RedirectToAction("Error");
	        }
        }

        public IActionResult Members() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0);
					return View(Users);
				} catch (Exception) {
					return RedirectToAction("Error");
				}
				
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Admin");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}
    }
}