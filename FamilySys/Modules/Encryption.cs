using System.Security.Cryptography;
using System.Text;

namespace FamilySys.Modules {
	public class Encryption {
		private readonly MD5 md5 = MD5.Create();

		public string Encrypt(string key)
		{
			byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(key));
			StringBuilder sBuilder = new();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}
