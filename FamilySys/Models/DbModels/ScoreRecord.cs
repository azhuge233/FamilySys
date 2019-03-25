using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class ScoreRecord {
		[Key]
		[MaxLength(5)]
		public string ID { get; set; }

		[Required]
		[MaxLength(10)]
		public string UserID { get; set; }

		[Required]
		[MaxLength(10)]
		public string HouseworkID { get; set; }

		[Required]
		[MaxLength(5)]
		public string RateID { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public int Score { get; set; }
	}
}
