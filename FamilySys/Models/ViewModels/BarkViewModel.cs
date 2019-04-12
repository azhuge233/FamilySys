using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels {
	public class BarkViewModel {

		public string Id { get; set; }

		public string Username { get; set; }

		[DisplayName("Bark推送地址")]
		[Required(ErrorMessage = "请填写Bark推送地址")]
		[MaxLength(50, ErrorMessage = "长度不能大于50")]
		public string Address { get; set; }

		[DisplayName("设备Key")]
		[Required(ErrorMessage = "请填写设备Key")]
		[MaxLength(40, ErrorMessage = "长度不能大于40")]
		public string Key { get; set; }

		[DisplayName("使用https")]
		[Required(ErrorMessage = "请选择是否支持https")]
		public bool Is_https { get; set; }
	}
}
