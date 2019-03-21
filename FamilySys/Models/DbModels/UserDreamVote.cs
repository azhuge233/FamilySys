using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class UserDreamVote {
		[Key]
		[MaxLength(5)]
		public string ID { get; set; }

		[Required]
		[MaxLength(10)]
		public string UserID { get; set; }

		[Required]
		[MaxLength(5)]
		public string DreamID { get; set; }

		[Required]
		public bool IsAgree { get; set; }

		[Required]
		public bool IsVeto { get; set; }
	}
}
