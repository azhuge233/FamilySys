using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class Dream {

		[Key]
		[Required]
		[MaxLength(5)]
		public string ID { get; set; }

		[MaxLength(10)]
		[Required]
		public string UserID { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public int Agree { get; set; }

		[Required]
		public int Veto { get; set; }
	}
}
