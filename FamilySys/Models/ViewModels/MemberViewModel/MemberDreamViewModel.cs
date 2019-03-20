using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels.MemberViewModel {
	public class MemberDreamViewModel {
		[Required]
		[DisplayName("ID")]
		public string ID { get; set; }

		[Required(ErrorMessage = "请填写你的梦想标题")]
		[DisplayName("梦想标题")]
		public string Title { get; set; }

		[Required(ErrorMessage = "请描述你的梦想")]
		[DisplayName("梦想内容")]
		public string Content { get; set; }
	}
}
