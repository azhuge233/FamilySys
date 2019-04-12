using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FamilySys.Modules {
	public class Barker {
		private string address;

		public void SetAddress(string Server, string Key, string Message, bool is_https = false) {
			address = "";
			address += is_https ? "https://" : "http://";
			address += Server + "/" + Key + "/" + Message.Replace(" ", "%20");
		}

		public void Bark() {
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(address);
			req.GetResponse();
		}
	}
}
