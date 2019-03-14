using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Models.ViewModels;
using FamilySys.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FamilySys.Controllers
{
    public class LoginController : Controller
    {
	    private readonly Encryption encryption;
	    private readonly FamilySysDbContext db;

	    public LoginController(FamilySysDbContext _db, Encryption _encryption)
	    {
		    db = _db;
		    encryption = _encryption;
	    }

		//校验用户名重名
	    public JsonResult UsernameValidationJsonResult(string username)
	    {
		    bool isExists = db.Users.Any(x => x.Username == username);
		    return Json(!isExists);
	    }

	    //视图部分
		public IActionResult Index()
		{
			if (HttpContext.Session.GetInt32("isAdmin") == 1)
			{
				return RedirectToAction("Index", "Admin");
			}
			else if(HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("Index", "Member");
			}
			else
			{
				return View();
			}
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel form)
        {
	        if (HttpContext.Session.GetInt32("isAdmin") == 1) {
		        return RedirectToAction("Index", "Admin");
	        } else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
		        return RedirectToAction("Index", "Member");
	        }
	        else
	        {
		        var username = form.Username;
		        var password = form.Password;
		        //if条件测试用，发布时删除
		        if (username != "admin")
		        {
			        password = encryption.Encrypt(password);
		        }

		        try
		        {
			        if (db.Users.Any(x => x.Username == username && x.Password == password))
			        {
				        var user = db.Users.Single(x => x.Username == username && x.Password == password);
				        HttpContext.Session.SetString("ID", user.ID);
				        HttpContext.Session.SetInt32("isAdmin", user.IsAdmin);
				        if (user.IsAdmin == 1)
				        {
					        return RedirectToAction("Index", "Admin");
				        }
				        else
				        {
					        return RedirectToAction("Index", "Member");
				        }
			        }
			        else
			        {
				        form.ErrMessage = "用户名或密码错误";
				        return View("Index", form);
			        }
		        }
		        catch
		        {
			        return RedirectToAction("Error", "Home");
		        }
	        }
        }

        public IActionResult Signup()
        {
	        if (HttpContext.Session.GetInt32("isAdmin") == 1) {
		        return RedirectToAction("Index", "Admin");
	        } else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
		        return RedirectToAction("Index", "Member");
	        }
	        else
	        {
		        var formWithID = new UserSignUpViewModel();
		        Random rd = new Random();
		        do
		        {
			        formWithID.ID = rd.Next(00000, 99999).ToString() + rd.Next(00000, 99999).ToString();
		        } while (db.Users.Any(x => x.ID == formWithID.ID));

		        return View(formWithID);
	        }
        }

        [HttpPost]
        public IActionResult Signup(UserSignUpViewModel form)
        {
	        var freshman = new User()
	        {
		        ID = form.ID,
		        IsAdmin = 0,
		        Username = form.Username,
		        Password = encryption.Encrypt(form.Password),
		        Sex = form.Sex,
		        Phone = form.Phone,
		        Mail = form.Mail
	        };

	        try
	        {
		        db.Users.Add(freshman);
		        db.SaveChanges();

				var loginform = new UserLoginViewModel()
				{
					Username = freshman.Username
				};
		        return View("Index", loginform);
	        }
	        catch (Exception)
	        {
		        return RedirectToAction("Error", "Home");
	        }
        }

    }
}