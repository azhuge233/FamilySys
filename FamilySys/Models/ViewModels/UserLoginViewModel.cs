using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels {
	public class UserLoginViewModel {
		[Required(ErrorMessage = "请填写用户名")]
		[MaxLength(20, ErrorMessage = "长度不能大于20")]
		[DisplayName("用户名")]
		public string Username { get; set; }

		[Required(ErrorMessage = "请填写密码")]
		[StringLength(20, ErrorMessage = "长度不能大于20位")]
		[DisplayName("密码")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string ErrMessage { get; set; }
	}
}
