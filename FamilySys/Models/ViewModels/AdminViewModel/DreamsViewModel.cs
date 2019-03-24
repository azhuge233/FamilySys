using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels.AdminViewModel {
	public class DreamsViewModel {
		public string ID { get; set; }

		public string Title { get; set; }

		public string Username { get; set; }

		public string UserID { get; set; }

		public int Agree { get; set; }

		public int Veto { get; set; }
	}
}
