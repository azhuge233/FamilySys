using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FamilySys.Modules {
	public class Encryption {
		private MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

		public string Encrypt(string key)
		{
			byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(key));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}
