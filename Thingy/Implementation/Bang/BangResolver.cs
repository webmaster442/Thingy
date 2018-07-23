using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Thingy.Implementation.Bang
{
    internal static class BangResolver
    {
        public static async Task<string> Resolve(string bangQuery)
        {
            if (!bangQuery.StartsWith("!")) return null;
            var encoded = HttpUtility.UrlEncode(bangQuery);
            var api = $"https://api.duckduckgo.com/?q={encoded}&format=json&pretty=1&no_redirect=1";

            using (WebClient client = new WebClient())
            {
                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.Proxy = defaultProxy;
                }

                var response = await client.DownloadStringTaskAsync(api);
                var parsedResponse = JsonConvert.DeserializeObject<BangResponse>(response);
                return parsedResponse.Redirect;
            }
        }
    }
}
