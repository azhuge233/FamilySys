using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class MonthlyRank {
		[Key]
		[MaxLength(10)]
		public string ID { get; set; }

		[Required]
		[MaxLength(10)]
		public string UserID { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public int Score { get; set; }

		[Required]
		public int Rank { get; set; }
	}
}
