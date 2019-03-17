using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models.DbModels;

namespace FamilySys.Models.ViewModels.AdminViewModel {
	public class AnnouncementViewModel {

		[DisplayName("标题")]
		[Required(ErrorMessage = "请填写公告标题")]
		public string Title { get; set; }

		[DisplayName("内容")]
		[Required(ErrorMessage = "请填写公告内容")]
		public string Content { get; set; }
	}
}
