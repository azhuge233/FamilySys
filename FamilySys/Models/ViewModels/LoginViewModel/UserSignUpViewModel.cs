using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32.SafeHandles;

namespace FamilySys.Models.ViewModels {
	public class UserSignUpViewModel {
		[Key]
		[DisplayName("ID")]
		[MaxLength(10)]
		public string ID { get; set; }

		[Required(ErrorMessage = "请填写用户名")]
		[DisplayName("用户名")]
		[StringLength(20, MinimumLength = 2, ErrorMessage = "用户名长度在2-20位之间")]
		[Remote("UsernameValidationJsonResult", "Login", ErrorMessage = "用户名已存在")]
		public string Username { get; set; }

		[DisplayName("密码")]
		[Required(ErrorMessage = "请填写密码")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密码长度在8-20位之间")]
		public string Password { get; set; }

		[DisplayName("性别")]
		[Required(ErrorMessage = "请填写性别")]
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

		public int IsAdmin { get; set; }

		public List<SelectListItem> SexList = new List<SelectListItem>()
		{
			new SelectListItem("男", "男"),
			new SelectListItem("女", "女")
		};
	}
}
