using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;

namespace Thingy.MusicPlayerCore
{
    public static class iTunesCoverDownloader
    {
        public static Task<byte[]> GetCoverFor(string query)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                        if (defaultProxy != null)
                        {
                            defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                            client.Proxy = defaultProxy;
                        }

                        var encoded = HttpUtility.UrlEncode(query);

                        var fulladdress = $"https://itunes.apple.com/search?term={encoded}&media=music&limit=1";

                        var response = client.DownloadString(fulladdress);

                        dynamic responseObject = JsonConvert.DeserializeObject(response);

                        string artwork = responseObject.results[0].artworkUrl100;
                        artwork = artwork.Replace("100x100", "600x600");

                        return client.DownloadData(artwork);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public static BitmapImage CreateBitmap(byte[] artworkData)
        {
            using (var ms = new MemoryStream(artworkData))
            {
                BitmapImage ret = new BitmapImage();
                ret.BeginInit();
                ret.CacheOption = BitmapCacheOption.OnLoad;
                ret.StreamSource = ms;
                ret.EndInit();
                ret.Freeze();
                return ret;
            }
        }
    }
}
