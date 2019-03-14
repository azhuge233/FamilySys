using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilySys.Controllers
{
    public class MemberController : Controller
    {
	    private readonly FamilySysDbContext db;

	    public MemberController(FamilySysDbContext _db)
	    {
		    db = _db;
	    }

	    public IActionResult Index()
        {
	        if (HttpContext.Session.GetInt32("isAdmin") == 1) {
		        return RedirectToAction("Index", "Admin");
	        } else if (HttpContext.Session.GetInt32("isAdmin") == 0)
	        {
		        var form = new MemberIndexViewModel()
		        {
			        ID = HttpContext.Session.GetString("ID"),
			        Username = db.Users.Single(x => x.ID == HttpContext.Session.GetString("ID")).Username
		        };

				return View(form);
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

        public IActionResult Logout()
        {
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
        }
    }
}