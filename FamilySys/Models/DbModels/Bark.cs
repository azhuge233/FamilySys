using System.ComponentModel.DataAnnotations;
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
