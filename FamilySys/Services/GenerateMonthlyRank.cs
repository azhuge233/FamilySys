using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;

namespace FamilySys.Services {
	public class GenerateMonthlyRank {
		internal interface IMonthlyRank {
			void Generate();
		}

		internal class MonthlyRank: IMonthlyRank {
			private readonly FamilySysDbContext db;

			public MonthlyRank(FamilySysDbContext _db) {
				db = _db;
			}

			private string GetRandomNumber10() {
				var rd = new Random();
				string ID = "";

				do {
					ID = rd.Next(10000, 99999).ToString() + rd.Next(10000, 99999).ToString();
				} while (db.MonthlyRanks.Any(x => x.ID == ID));

				return ID;
			}

			public void Generate() {
				var now = DateTime.Now;
				var ThisMonthRecord = db.ScoreRecords.Where(x => x.Date.Month == (now.Month == 1 ? 12 : now.Month - 1));
				var Users = db.Users.Where(x => x.IsAdmin == 0);
				
				var ScoreList = new Dictionary<string, int>();

				foreach (var user in Users) {  //初始化所有 用户-分数 对
					ScoreList.Add(user.ID, 0);
				}

				foreach (var record in ThisMonthRecord) { //将记录的分数加到对应用户ID
					ScoreList[record.UserID] += record.Score;
				}

				ScoreList = ScoreList.OrderByDescending(x => x.Value).ToDictionary(p => p.Key, o => o.Value); //按分数降序排序

				int rank = 1;
				foreach (var score in ScoreList) { //将排序后的每个元组写入数据库
					db.MonthlyRanks.Add(
						new Models.DbModels.MonthlyRank() {
							ID = GetRandomNumber10(),
							UserID = score.Key,
							Date = DateTime.Now,
							Rank = rank,
							Score = score.Value
						}
					);
					rank += 1;
				}

				db.SaveChanges();
			}
		}
	}
}
