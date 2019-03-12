using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
    public class LoginController : Controller
    {
	    private readonly FamilySysDbContext db;

	    public LoginController(FamilySysDbContext _db)
	    {
		    db = _db;
	    }

	    public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel form)
        {
	        var username = form.Username;
	        var password = form.Password;

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
					} else
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
}