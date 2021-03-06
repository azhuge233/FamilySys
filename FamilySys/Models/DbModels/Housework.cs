﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class Housework {

		[Key]
		[MaxLength(10)]
		public string ID { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public int Type { get; set; }

		[Range(0, 10)]
		public int Score { get; set; }
		
		[Required]
		[MaxLength(10)]
		public string FromID { get; set; }

		[MaxLength(10)]
		public string ToID { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public DateTime ModifyDate { get; set; }

		[Required]
		public bool IsDone { get; set; }
	}
}
