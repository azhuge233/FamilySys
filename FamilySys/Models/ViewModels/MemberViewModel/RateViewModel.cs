using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels.MemberViewModel {
	public class RateViewModel {
		[DisplayName("ID")]
		[MaxLength(5)]
		public string ID { get; set; }

		[Required]
		[DisplayName("家务ID")]
		public string HouseworkID { get; set; }

		[Required(ErrorMessage = "请填写评分")]
		[DisplayName("评分")]
		public int Star { get; set; }

		[DisplayName("备注")]
		public string Comment { get; set; }
	}
}
