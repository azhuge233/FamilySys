using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Sakura.AspNetCore;

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
			ViewBag.ID = HttpContext.Session.GetString("ID");
		}

		public string GetRandomNum() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString() + rd.Next(10000, 99999).ToString();
			} while (db.Houseworks.Any(x => x.ID == ID));

			return ID;
		}

		public string GetRandomNum5() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString();
			} while (db.Dreams.Any(x => x.ID == ID));

			return ID;
		}

		public string GetRandomNum5Vote() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString();
			} while (db.UserDreamVotes.Any(x => x.ID == ID));

			return ID;
		}

		public string GetRandomNum5Rate() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString();
			} while (db.Rates.Any(x => x.ID == ID));

			return ID;
		}

		public string GetRandomNum5ScoreRecord() {
			Random rd = new Random();
			string ID = "";

			do {
				ID = rd.Next(10000, 99999).ToString();
			} while (db.ScoreRecords.Any(x => x.ID == ID));

			return ID;
		}

		public bool GenerateYearAndMonthList() {
			if (!db.MonthlyRanks.Any()) return false;

			ViewBag.MonthList = new SelectList(new List<SelectListItem>() {
					new SelectListItem("1", "1"),
					new SelectListItem("2", "2"),
					new SelectListItem("3", "3"),
					new SelectListItem("4", "4"),
					new SelectListItem("5", "5"),
					new SelectListItem("6", "6"),
					new SelectListItem("7", "7"),
					new SelectListItem("8", "8"),
					new SelectListItem("9", "9"),
					new SelectListItem("10", "10"),
					new SelectListItem("11", "11"),
					new SelectListItem("12", "12")

				}, "Value", "Text"
			);

			var startYear = db.MonthlyRanks.Select(x => x).OrderBy(x => x.Date).First().Date.Year;
			var YearList = new List<SelectListItem>();

			for (int i = startYear; i <= DateTime.Now.Year; i++) {
				YearList.Add(new SelectListItem(i.ToString(), i.ToString()));
			}

			ViewBag.YearList = new SelectList(YearList,"Value", "Text");

			return true;
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
						Mail = me.Mail,
						Score = me.Score
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

				TempData["Success"] = "<script>alert(\'信息已修改\');</script>";
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
					TempData["WrongPwd"] = "原密码输入错误";
					return RedirectToAction("MyInfo");
				}
			}
			catch(Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		[HttpGet]
		public IActionResult Members(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Members", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0).OrderBy(x => x.ID);

					var UsersPagedList = Users.ToPagedList(8, page);

					return View(UsersPagedList);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpGet]
		public IActionResult ShowAnnouncements(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("ShowAnnouncements", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var Annos = db.Announcements.Where(x => x.ID == x.ID).OrderByDescending(x => x.Date);

					var AnnosPagedList = Annos.ToPagedList(8, page);

					return View(AnnosPagedList);
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

					Anno.Content = Anno.Content.Replace("\n", "<br />");

					return View(Anno);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpGet]
		public IActionResult ShowHouseworks(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

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

				if (form.Type == 2) {
					if (me.Score <= 0) {
						TempData["ErrMsg"] = "<script>alert(\'您的积分不足，无法发布个人事务\')</script>";
						return RedirectToAction("MyHouseworks");
					} else if (me.Score - form.Score < 0) {
						TempData["CannotPublish"] = "<script>alert(\'您的积分小于事务分值，无法发布事务 #" + form.ID + "\')</script>";
						return RedirectToAction("PublishHousework");
					} else {
						me.Score -= form.Type == 2 ? form.Score : 0;
					}
				}

				db.Houseworks.Add(newHousework);
				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'事务 #" + newHousework.ID + " 已成功发布\')</script>";
				return RedirectToAction("MyHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowHouseworkDetails(string ID, string FromPage) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				ViewBag.FromPage = FromPage;

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

		[HttpPost]
		public IActionResult HouseworkOperation(string ID, string action, string FromPage) {
			try {
				var housework = db.Houseworks.Single(x => x.ID == ID);
				var myID = HttpContext.Session.GetString("ID");

				if (action == "1") {
					housework.ToID = myID;
					TempData["Success"] = "<script>alert(\'已接受事务 #" + housework.ID + "\')</script>";
				} else if (action == "2") {
					housework.ToID = null;
					TempData["Success"] = "<script>alert(\'已放弃事务 #" + housework.ID + "\')</script>";
				} else if(action == "3") {
					housework.IsDone = true;
					TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已完成，将通知事务发布人\')</script>";
				} else if(action == "4") {
					if (housework.Type == 2) { //返还积分
						var me = db.Users.Single(x => x.ID == myID);
						me.Score += housework.Score;
						TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已删除，返还 " + housework.Score + " 积分\')</script>";
					} else {
						TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已删除\')</script>";
					}
					db.Houseworks.Remove(housework);
				}	

				db.SaveChanges();

				return FromPage == "1" ? RedirectToAction("ShowHouseworks") : RedirectToAction("MyHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult MyHouseworks() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var myID = HttpContext.Session.GetString("ID");
					var houseworks = db.Houseworks.Where(x => x.FromID == myID || x.ToID == myID);
					var houseworkShowcase = new List<HouseworkShowcaseViewModel>();

					foreach (var housework in houseworks) {
						houseworkShowcase.Add(new HouseworkShowcaseViewModel() {
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
								ToUsername = housework.ToID == null
									? "无"
									: db.Users.Single(x => x.ID == housework.ToID).Username,
								IsRated = db.Rates.Any(x => x.HouseworkID == housework.ID)
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

		public IActionResult EditHousework(string ID, string FromPage) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var editWork = db.Houseworks.Single(x => x.ID == ID);

					var editWorkViewModel = new HouseworkPublishViewModel() {
						ID = editWork.ID,
						Title = editWork.Title,
						Content = editWork.Content,
						Type = editWork.Type,
						Score = editWork.Score
					};

					ViewBag.FromPage = FromPage;

					return View(editWorkViewModel);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoEditHousework(HouseworkPublishViewModel form, string FromPage) {
			try {
				var housework = db.Houseworks.Single(x => x.ID == form.ID);
				var me = db.Users.Single(x => x.ID == HttpContext.Session.GetString("ID"));
				var differ = housework.Score - form.Score;

				if (differ < 0 && (me.Score + differ) < 0) {
					TempData["ErrMsg"] = "<script>alert(\'无法修改事务 #" + housework.ID + "，账户积分不足 \')</script>";
					return FromPage == "1" ? RedirectToAction("ShowHouseworks") : RedirectToAction("MyHouseworks");
				}

				if (housework.Type == 2 && differ != 0) { //修改个人事务积分后返还或扣除积分
						me.Score += differ;
				}

				housework.Title = form.Title;
				housework.Type = form.Type;
				housework.Content = form.Content;
				housework.Score = form.Score;

				db.SaveChanges();
				if (housework.Type == 2) {
					if (differ < 0) {
						TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已修改，积分 " + differ +
						                      " 分\')</script>";
					} else if (differ > 0) {
						TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已修改，返还" + differ +
						                      " 积分\')</script>";
					}
				} else {
					TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + " 已修改\')</script>";
				}

				return FromPage == "1" ? RedirectToAction("ShowHouseworks") : RedirectToAction("MyHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult MyDream() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var myID = HttpContext.Session.GetString("ID");
					var newDream = new MemberDreamViewModel();

					if (db.Dreams.Any(x => x.UserID == myID)) {
						var oldDream = db.Dreams.Single(x => x.UserID == myID);
						newDream.ID = oldDream.ID;
						newDream.Title = oldDream.Title;
						newDream.Content = oldDream.Content;
					} else {
						newDream.ID = GetRandomNum5();
					}

					return View(newDream);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult UploadDream(MemberDreamViewModel form) {
			try {

				if (db.Dreams.Any(x => x.ID == form.ID)) {

					var dream = db.Dreams.Single(x => x.ID == form.ID);

					dream.Title = form.Title;
					dream.Content = form.Content;
					dream.Agree = 1;
					dream.Veto = 0;

					db.UserDreamVotes.RemoveRange(db.UserDreamVotes.Where(x => x.DreamID == dream.ID));
				} else {
					var newDream = new Dream() {
						ID = form.ID,
						Title = form.Title,
						Content = form.Content,
						UserID = HttpContext.Session.GetString("ID"),
						Agree = 1,
						Veto = 0
					};

					db.Dreams.Add(newDream);
				}

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'已设置家庭梦想\');</script>";
				return RedirectToAction("ShowDreams");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowDreams() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var myID = HttpContext.Session.GetString("ID");
					var dreams = db.Dreams.Select(x => x).Where(x => x.UserID != myID);

					if (!db.Dreams.Any(x => x.UserID == myID)) {
						ViewBag.myDreamTitle = "无";
						ViewBag.myDreamAgree = 0;
						ViewBag.myDreamVeto = 0;
					} else {
						var myDream = db.Dreams.Single(x => x.UserID == myID);
						ViewBag.myDreamTitle = myDream.Title;
						ViewBag.myDreamAgree = myDream.Agree;
						ViewBag.myDreamVeto = myDream.Veto;
					}
					ViewBag.UserCount = db.Users.Count() - 1;

					var dreamsList = new List<ShowDreamsViewModel>();

					foreach (var dream in dreams) {
						dreamsList.Add(
							new ShowDreamsViewModel() {
								ID = dream.ID,
								Title = dream.Title,
								Agree = dream.Agree,
								Veto = dream.Veto,
								Username = db.Users.Single(x => x.ID == dream.UserID).Username,
								IsAgree = db.UserDreamVotes.Any(x => x.UserID == myID && x.DreamID == dream.ID) ? db.UserDreamVotes.Single(x => x.UserID == myID && x.DreamID == dream.ID).IsAgree.ToString() : null,
								IsVeto = db.UserDreamVotes.Any(x => x.UserID == myID && x.DreamID == dream.ID) ? db.UserDreamVotes.Single(x => x.UserID == myID && x.DreamID == dream.ID).IsVeto.ToString() : null,
							}
						);
					}

					return View(dreamsList.AsQueryable());
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult VoteDreams(string ID, string action) {
			try {
				var dream = db.Dreams.Single(x => x.ID == ID);
				var myID = HttpContext.Session.GetString("ID");

				if (action == "1") {
					dream.Agree += 1;

					if (db.UserDreamVotes.Any(x => x.DreamID == ID && x.UserID == myID)) {
						var vote = db.UserDreamVotes.Single(x => x.DreamID == ID && x.UserID == myID);
						vote.IsAgree = true;
					} else {
						var newVote = new UserDreamVote() {
							ID = GetRandomNum5Vote(),
							UserID = myID,
							DreamID = dream.ID,
							IsAgree = true,
							IsVeto = false
						};

						db.UserDreamVotes.Add(newVote);
					}

					TempData["Success"] = "<script>alert(\'已通过家庭梦想 \"" + dream.Title + "\"\');</script>";
				} else if(action == "2") {
					dream.Agree -= 1;

					var vote = db.UserDreamVotes.Single(x => x.DreamID == ID && x.UserID == myID);
					vote.IsAgree = false;

					TempData["Success"] = "<script>alert(\'已取消通过家庭梦想 \"" + dream.Title + "\"\');</script>";
				} else if(action == "3") {
					dream.Veto += 1;

					if (db.UserDreamVotes.Any(x => x.DreamID == ID && x.UserID == myID)) {
						var vote = db.UserDreamVotes.Single(x => x.DreamID == ID && x.UserID == myID);
						vote.IsVeto = true;
					} else {
						var newVote = new UserDreamVote() {
							ID = GetRandomNum5Vote(),
							UserID = myID,
							DreamID = dream.ID,
							IsAgree = false,
							IsVeto = true
						};

						db.UserDreamVotes.Add(newVote);
					}

					TempData["Success"] = "<script>alert(\'已否决家庭梦想 \"" + dream.Title + "\"\');</script>";
				} else if(action == "4") {
					dream.Veto -= 1;

					var vote = db.UserDreamVotes.Single(x => x.DreamID == ID && x.UserID == myID);
					vote.IsVeto = false;

					TempData["Success"] = "<script>alert(\'已取消否决家庭梦想 \"" + dream.Title + "\"\');</script>";
				}

				db.SaveChanges();

				return RedirectToAction("ShowDreams");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult Rate(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {

					var rate = new RateViewModel() {
						ID = GetRandomNum5Rate(),
						HouseworkID = ID
					};

					return View(rate);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoRate(RateViewModel form) {
			try {

				var myID = HttpContext.Session.GetString("ID");
				var housework = db.Houseworks.Single(x => x.ID == form.HouseworkID);

				if (form.Star == 0) {
					TempData["ErrMsg"] = "<script>alert(\'请填写评分\')</script>";
					return RedirectToAction("Rate");
				} else {
					var newRate = new Rate() {
						ID = form.ID,
						Comment = form.Comment ?? "无",
						Star = form.Star,
						HouseworkID = form.HouseworkID,
						FromID = myID,
						ToID = housework.ToID
					};

					db.Rates.Add(newRate);

					if (newRate.Star >= 3) { //计算积分
						var ToUser = db.Users.Single(x => x.ID == housework.ToID);
						ToUser.Score += housework.Score;
						TempData["Success"] = "<script>alert(\'已评价事务 #" + form.HouseworkID + " ，用户 " + ToUser.Username + " 积分 +" + housework.Score + "\')</script>";
					} else if (housework.Type == 2 && newRate.Star < 3) {
						var FromUser = db.Users.Single(x => x.ID == housework.FromID);
						FromUser.Score += housework.Score;
						TempData["Success"] = "<script>alert(\'已评价事务 #" + form.HouseworkID + "，已返还" + housework.Score + " 积分\')</script>";
					} else if(housework.Type == 1 && newRate.Star < 3) {
						TempData["Success"] = "<script>alert(\'已评价事务 #" + form.HouseworkID + "\')</script>";
					}

					var newRecord = new ScoreRecord() {
						ID = GetRandomNum5ScoreRecord(),
						UserID = housework.ToID,
						HouseworkID = housework.ID,
						RateID = form.ID,
						Score = form.Star >= 3 ? housework.Score : 0,
						Date = DateTime.Now
					};

					db.ScoreRecords.Add(newRecord);
				}

				db.SaveChanges();

				return RedirectToAction("MyHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult CheckRate(string ID, string FromPage) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {

					var rate = db.Rates.Single(x => x.HouseworkID == ID);

					var newRateModel = new RateViewModel() {
						ID = rate.ID,
						Comment = rate.Comment,
						Star = rate.Star,
						HouseworkID = rate.HouseworkID
					};

					ViewBag.FromPage = FromPage;

					return View(newRateModel);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpGet]
		public IActionResult ShowRecords(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					var records = db.ScoreRecords.Select(x => x);
					var RecordList = new List<RecordViewModel>();

					foreach (var record in records) {
						RecordList.Add(
							new RecordViewModel() {
								ID = record.ID,
								Username = db.Users.Single(x => x.ID == record.UserID).Username,
								HouseworkName = db.Houseworks.Single(x => x.ID == record.HouseworkID).Title,
								Star = db.Rates.Single(x => x.ID == record.RateID).Star,
								Score = record.Score
							}
						);
					}

					var RecordPagedList = RecordList.ToPagedList(8, page);

					return View(RecordPagedList);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpGet]
		public IActionResult ShowRanks(string year = null, string month = null) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("Index", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				try {
					int Year = DateTime.Now.Year;
					int Month = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
					if (year != null) Year = Convert.ToInt32(year);
					if (month != null) Month = Convert.ToInt32(month);

					bool hasRecord = GenerateYearAndMonthList();

					if (!hasRecord) {
						ViewBag.noRecord = true;

						return RedirectToAction("NoRecord");
					}

					if (!db.MonthlyRanks.Any(x => x.Date.Year == Year && x.Date.Month == Month)) {
						ViewBag.IsNull = true;

						return View();
					} else {
						ViewBag.IsNull = false;
						ViewBag.Year = Year;
						ViewBag.Month = Month;

						var ranks = db.MonthlyRanks.Where(x => x.Date.Year == Year && x.Date.Month == Month).OrderBy(x => x.Rank);

						var ranksList = new List<RankModel>();

						foreach (var rank in ranks) {
							ranksList.Add(
								new RankModel() {
									Username = db.Users.Single(x => x.ID == rank.UserID).Username,
									Score = rank.Score
								}
							);
						}

						return View(ranksList.AsQueryable());
					}
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult NoRecord() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				return RedirectToAction("NoRecord", "Admin");
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				CommonWork();

				return View();
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}
	}
}