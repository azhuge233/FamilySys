using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.DbModels {
	public class User {
		public string ID { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string Sex { get; set; }

		public string Phone { get; set; }

		public string Mail { get; set; }
	}
}
