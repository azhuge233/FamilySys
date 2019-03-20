using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilySys.Models.ViewModels.MemberViewModel {
	public class MyHouseworksViewModel {
		public string ID { get; set; }

		public string Title { get; set; }

		public int Type { get; set; }

		public int Score { get; set; }

		public string FromUsername { get; set; }

		public string ToUsername { get; set; }

		public DateTime Date { get; set; }

		public bool IsDone { get; set; }
	}
}
