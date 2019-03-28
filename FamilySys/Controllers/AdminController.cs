using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Models.ViewModels;
using FamilySys.Models.ViewModels.AdminViewModel;
using FamilySys.Models.ViewModels.MemberViewModel;
using FamilySys.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sakura.AspNetCore;

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

			ViewBag.YearList = new SelectList(YearList, "Value", "Text");

			return true;
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

				TempData["Success"] = "<script>alert(\'信息已修改\');</script>";
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
					var user = db.Users.Single(x => x.IsAdmin == 1);

					user.Password = encryption.Encrypt(form.NewPassword);
					db.SaveChanges();

					TempData["msg"] = "<script>alert('密码已修改，请重新登录');</script>";
					return RedirectToAction("Logout");
				} else {
					TempData["WrongPwd"] = "原密码输入错误";
					return RedirectToAction("MyInfo");
				}
			} catch(Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		[HttpGet]
		public IActionResult Members(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var Users = db.Users.Where(x => x.IsAdmin == 0);

					var UsersPagedList = Users.ToPagedList(8, page);

					return View(UsersPagedList);
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

		[HttpGet]
		public IActionResult ShowAnnouncements(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var Annos = db.Announcements.Where(x => x.ID == x.ID).OrderByDescending(x => x.Date);

					var AnnosPagedList = Annos.ToPagedList(8, page);

					return View(AnnosPagedList);
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
					Content = form.Content
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

				Anno.Content = Anno.Content.Replace("\n", "<br />");

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

		[HttpGet]
		public IActionResult ShowHouseworks(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
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
				
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowHouseworks", "Member");
			} else {
				return RedirectToAction("ShowHouseworks", "Home");
			}
		}

		public IActionResult ChangeRate(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var rate = db.Rates.Single(x => x.HouseworkID == ID);

					var newRate = new RateViewModel() {
						ID = rate.ID,
						Star = 0,
						Comment = rate.Comment,
						HouseworkID = rate.HouseworkID
					};

					return View(newRate);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowHouseworks", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoChangeRate(RateViewModel form) {
			try {
				if (form.Star == 0) {
					TempData["ErrMsg"] = "<script>alert(\'请填写评分\');</script>";
					return RedirectToAction("ShowHouseworks");
				}

				var rate = db.Rates.Single(x => x.ID == form.ID);
				var housework = db.Houseworks.Single(x => x.ID == form.HouseworkID);
				var fromUser = db.Users.Single(x => x.ID == rate.FromID);
				var toUser = db.Users.Single(x => x.ID == rate.ToID);

				rate.Comment = form.Comment ?? "无";
				if (housework.Type == 1) { //公共事务
					if (form.Star < 3 && rate.Star >= 3) {
						toUser.Score -= housework.Score;
					} else if (form.Star >= 3 && rate.Star < 3) {
						toUser.Score += housework.Score;
					}
				} else { //个人事务
					if (form.Star < 3 && rate.Star >= 3) {
						toUser.Score -= housework.Score;
						fromUser.Score += housework.Score;
					} else if (form.Star >= 3 && rate.Star < 3) {
						toUser.Score += housework.Score;
						fromUser.Score -= housework.Score;
					}
				}
				rate.Star = form.Star;

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'已修改评价 #" + rate.ID + "\');</script>";
				return RedirectToAction("ShowHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\');</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ChangeHousework(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var housework = db.Houseworks.Single(x => x.ID == ID);
					var users = db.Users.Select(x => x);

					var newHouseworkViewModel = new HouseworkEditViewModel(users) {
						ID = housework.ID,
						Title = housework.Title,
						Content = housework.Content,
						Score = housework.Score,
						Type = housework.Type,
						FromID = db.Users.Single(x => x.ID == housework.FromID).ID,
						ToID = housework.ToID == null ? "无" : db.Users.Single(x => x.ID == housework.ToID).ID
					};

					return View(newHouseworkViewModel);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}

			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowHouseworks", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpPost]
		public IActionResult DoEditHousework(HouseworkEditViewModel form) {
			try {
				var housework = db.Houseworks.Single(x => x.ID == form.ID);
				var oldUser =  db.Users.Single(x => x.ID == housework.FromID);
				var curUser =  db.Users.Single(x => x.ID == form.FromID);

				housework.Title = form.Title;
				housework.Content = form.Content;
				housework.ToID = form.ToID;
				housework.ModifyDate = DateTime.Now;

				if (housework.Type == form.Type) { //没改变事务类型
					if (housework.Type == 2) { //个人事务
						if (housework.FromID == form.FromID) { //发布人没变
							oldUser.Score += housework.Score;
							oldUser.Score -= form.Score;
							if (oldUser.Score < 0) {
								TempData["ErrMsg"] = "<script>alert(\'用户 " + oldUser.Username + " 的积分不足，修改失败\')</script>";
								return RedirectToAction("ShowHouseworks");
							}
						} else { //发布人改变
							oldUser.Score += housework.Score;
							curUser.Score -= form.Score;
							if (curUser.Score < 0) {
								TempData["ErrMsg"] = "<script>alert(\'用户 " + curUser.Username + " 的积分不足，修改失败\')</script>";
								return RedirectToAction("ShowHouseworks");
							}
						}
					}
				} else {
					if (form.Type == 1 && housework.Type == 2) { //个人变公共
						oldUser.Score += housework.Score;
					} else if (form.Type == 2 && housework.Type == 1) { //公共变个人
						if (housework.FromID == form.FromID) { //发布人没变
							oldUser.Score -= form.Score;
							if (oldUser.Score < 0) {
								TempData["ErrMsg"] = "<script>alert(\'用户 " + oldUser.Username + " 的积分不足，修改失败\')</script>";
								return RedirectToAction("ShowHouseworks");
							}
						} else { //发布人改变
							curUser.Score -= form.Score;
							if (curUser.Score < 0) {
								TempData["ErrMsg"] = "<script>alert(\'用户 " + curUser.Username + " 的积分不足，修改失败\')</script>";
								return RedirectToAction("ShowHouseworks");
							}
						}
					}
				}

				housework.Type = form.Type;
				housework.Score = form.Score;
				housework.FromID = form.FromID;

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'事务 #" + housework.ID + "修改成功\')</script>";
				return RedirectToAction("ShowHouseworks");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		public IActionResult ShowDreams() {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var dreams = db.Dreams.Select(x => x);
					var DreamList = new List<DreamsViewModel>();

					foreach (var dream in dreams) {
						DreamList.Add(
							new DreamsViewModel() {
								ID = dream.ID,
								Title = dream.Title,
								UserID = dream.UserID,
								Username = db.Users.Single(x => x.ID == dream.UserID).Username,
								Agree = dream.Agree,
								Veto = dream.Veto
							}
						);
					}

					ViewBag.UserCount = db.Users.Count() - 1;

					return View(DreamList.AsQueryable());
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}

			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("MyDream", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult ChangeDream(string ID) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					var dream = db.Dreams.Single(x => x.ID == ID);

					var editDream = new MemberDreamViewModel() {
						ID = dream.ID,
						Title = dream.Title,
						Content = dream.Content
					};

					return View(editDream);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}

			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("MyDream", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		public IActionResult DoChangeDream(MemberDreamViewModel form) {
			try {
				var dream = db.Dreams.Single(x => x.ID == form.ID);

				dream.Title = form.Title;
				dream.Content = form.Content;
				dream.Agree = 1;
				dream.Veto = 0;

				db.UserDreamVotes.RemoveRange(db.UserDreamVotes.Where(x => x.DreamID == dream.ID));

				db.SaveChanges();

				TempData["Success"] = "<script>alert(\'已修改家庭梦想 #" + dream.ID + "\')</script>";
				return RedirectToAction("ShowDreams");
			} catch (Exception ex) {
				TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
				return RedirectToAction("Error");
			}
		}

		[HttpGet]
		public IActionResult ShowRecords(int page = 1) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
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

					var RecordPageList = RecordList.ToPagedList(8, page);

					return View(RecordPageList);
				} catch (Exception ex) {
					TempData["ErrMsg"] = "<script>alert(\'" + ex.Message.ToString() + "\')</script>";
					return RedirectToAction("Error");
				}
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowRecords", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}

		[HttpGet]
		public IActionResult ShowRanks(string year = null, string month = null) {
			if (HttpContext.Session.GetInt32("isAdmin") == 1) {
				try {
					int Year = DateTime.Now.Year;
					int Month = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
					if (year != null) Year = Convert.ToInt32(year);
					if (month != null) Month = Convert.ToInt32(month);

					bool hasRecord = GenerateYearAndMonthList();

					if (!hasRecord) {
						ViewBag.noRecord = true;

						return View();
					} else {
						ViewBag.noRecord = false;
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
			} else if (HttpContext.Session.GetInt32("isAdmin") == 0) {
				return RedirectToAction("ShowRanks", "Member");
			} else {
				return RedirectToAction("nonMemberAlarm", "Home");
			}
		}
	}
}