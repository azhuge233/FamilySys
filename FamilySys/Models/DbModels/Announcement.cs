using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class Announcement {
		[Key]
		[MaxLength(5)]
		public string ID { get; set; }

		[Required]
		public DateTime Date { get; set; }

		public DateTime ModifyDate { get; set; }

		[Required] 
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
