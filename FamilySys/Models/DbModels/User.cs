using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class User {
		[Key]
		[MaxLength(10)]
		public string ID { get; set; }

		[Required]
		[MaxLength(20)]
		public string Username { get; set; }

		[Required]
		[MaxLength(32)]
		public string Password { get; set; }

		[Required]
		[MaxLength(3)]
		public string Sex { get; set; }

		[MaxLength(15)]
		public string Phone { get; set; }

		[Required]
		[MaxLength(50)]
		public string Mail { get; set; }

		[Required]
		public int IsAdmin { get; set; }
	}
}
