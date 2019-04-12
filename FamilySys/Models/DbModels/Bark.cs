using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FamilySys.Models.DbModels {
	public class Bark {

		[Key]
		[MaxLength(10)]
		public string Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Address { get; set; }

		[Required]
		[MaxLength(40)]
		public string Key { get; set; }

		[Required]
		public bool is_https { get; set; }
	}
}
