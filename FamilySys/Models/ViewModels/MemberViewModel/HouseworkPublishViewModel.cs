using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FamilySys.Models.ViewModels.MemberViewModel {
	public class HouseworkPublishViewModel {

		[Required]
		public string ID { get; set; }

		[Required(ErrorMessage = "请填写家务标题")]
		[DisplayName("标题")]
		public string Title { get; set; }

		[Required(ErrorMessage = "请填写家务内容")]
		[DisplayName("内容")]
		public string Content { get; set; }

		[Required(ErrorMessage = "请选择家务类型")]
		[DisplayName("类型")]
		public int Type { get; set; }

		[Required(ErrorMessage = "请填写家务分数")]
		[DisplayName("分值")]
		[Range(0, 10)]
		public int Score { get; set; }

		public List<SelectListItem> TypeList = new List<SelectListItem>()
		{
			new SelectListItem("公共事务", "1"),
			new SelectListItem("个人事务", "2")
		};

		public List<SelectListItem> ScoreList = new List<SelectListItem>()
		{
			new SelectListItem("1", "1"),
			new SelectListItem("2", "2"),
			new SelectListItem("3", "3"),
			new SelectListItem("4", "4"),
			new SelectListItem("5", "5"),
			new SelectListItem("6", "6"),
			new SelectListItem("7", "7"),
			new SelectListItem("8", "8"),
			new SelectListItem("9", "9"),
			new SelectListItem("10", "10")
		};
	}
}
