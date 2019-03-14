using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels {
	public class MemberChangePwdViewModel {
		[DisplayName("ID")]
		public string ID { get; set; }

		[DisplayName("用户名")]
		public string Username { get; set; }

		[DisplayName("原密码")]
		[Required(ErrorMessage = "请填写原密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		public string Password { get; set; }

		[DisplayName("新密码")]
		[Required(ErrorMessage = "请填写原密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		public string NewPassword { get; set; }

		[DisplayName("再次输入新密码")]
		[Required(ErrorMessage = "请填写原密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		[Compare("NewPassword", ErrorMessage = "与新密码不一致")]
		public string RetypePassword { get; set; }
	}
}
