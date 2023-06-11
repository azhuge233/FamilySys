using System.Net.Http;
using System.Threading.Tasks;

namespace FamilySys.Modules {
	public class Barker {
		public async Task Bark(string Url) {
			var client = new HttpClient();
			_ = await client.GetAsync(Url);
		}
	}
}
