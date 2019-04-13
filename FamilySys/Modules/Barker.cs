using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FamilySys.Modules {
	public class Barker {
		public void Bark(string Url) {
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(Url);
			req.GetResponse();
		}
	}
}
