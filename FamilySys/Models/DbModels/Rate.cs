using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models.ViewModels.MemberViewModel;

namespace FamilySys.Models.DbModels {
	public class Rate {
		[Key]
		[MaxLength(5)]
		public string ID { get; set; }

		[Required]
		[MaxLength(10)]
		public string HouseworkID { get; set; }

		[Required]
		[MaxLength(10)]
		public string FromID { get; set; }

		[Required]
		[MaxLength(10)]
		public string ToID { get; set; }

		[Required]
		public int Star { get; set; }

		[Required]
		public string Comment { get; set; }
	}
}
