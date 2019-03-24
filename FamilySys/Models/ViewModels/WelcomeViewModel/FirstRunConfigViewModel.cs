using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FamilySys.Models.ViewModels.WelcomeViewModel {
	public class FirstRunConfigViewModel {

		[DisplayName("ID")]
		public string ID { get; } = "0000000000";

		[DisplayName("用户名")]
		public string Username { get; } = "admin";

		[DisplayName("密码")]
		[Required(ErrorMessage = "请填写密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		public string Password { get; set; }

		[DisplayName("确认密码")]
		[Required(ErrorMessage = "请重新输入密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		[Compare("Password", ErrorMessage = "与密码不一致")]
		public string RetypePassword { get; set; }

		[DisplayName("性别")]
		[Required(ErrorMessage = "请选择性别")]
		[MaxLength(3)]
		public string Sex { get; set; }

		[DisplayName("电话")]
		[MaxLength(15, ErrorMessage = "电话号码长度在15位以内")]
		public string Phone { get; set; }

		[DisplayName("邮箱")]
		[Required(ErrorMessage = "请填写邮箱")]
		[StringLength(50, MinimumLength = 6, ErrorMessage = "邮箱地址在6-50位之间")]
		[MaxLength(50)]
		public string Mail { get; set; }

		public List<SelectListItem> SexList = new List<SelectListItem>()
		{
			new SelectListItem("男", "男"),
			new SelectListItem("女", "女")
		};
	}
}
