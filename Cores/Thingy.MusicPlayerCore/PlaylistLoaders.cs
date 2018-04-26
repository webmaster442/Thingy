using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Thingy.MusicPlayerCore
{
    public static class PlaylistLoaders
    {
        private async static Task<TextReader> LoadFile(string file)
        {
            if (file.StartsWith("http://") || file.StartsWith("https://"))
            {
                using (var client = new WebClient())
                {
                    IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                    if (defaultProxy != null)
                    {
                        defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                        client.Proxy = defaultProxy;
                    }

                    var response = await client.DownloadStringTaskAsync(new Uri(file));
                    return new StringReader(response);
                }
            }
            else return File.OpenText(file);
        }

        public static async Task<IEnumerable<string>> LoadM3u(string file)
        {
            List<string> ret = new List<string>();
            string filedir = System.IO.Path.GetDirectoryName(file);
            string line;
            using (var content = await LoadFile(file))
            {
                do
                {
                    line = content.ReadLine();
                    if (line == null) continue;
                    if (line.StartsWith("#")) continue;
                    if (line.StartsWith("http://") || line.StartsWith("https://"))
                    {
                        ret.Add(line);
                    }
                    else if (line.Contains(":\\") || line.StartsWith("\\\\"))
                    {
                        if (!File.Exists(line)) continue;
                        ret.Add(line);
                    }
                    else
                    {
                        string f = Path.Combine(filedir, line);
                        if (!File.Exists(f)) continue;
                        ret.Add(f);
                    }
                }
                while (line != null);
            }
            return ret;
        }

        public static async Task<IEnumerable<string>> LoadWPL(string file)
        {

            var content = await LoadFile(file);
            var doc = XDocument.Load(content).Descendants("body").Elements("seq").Elements("media");
            List<string> ret = new List<string>();
            foreach (var media in doc)
            {
                var src = media.Attribute("src").Value;
                ret.Add(src);
            }
            return ret;
        }

        public static async Task<IEnumerable<string>> LoadASX(string file)
        {
            var content = await LoadFile(file);
            var doc = XDocument.Load(content).Descendants("asx").Elements("entry").Elements("ref");
            List<string> ret = new List<string>();
            foreach (var media in doc)
            {
                var src = media.Attribute("href").Value;
                ret.Add(src);
            }
            return ret;
        }


        public static async Task<IEnumerable<string>> LoadPls(string file)
        {
            string filedir = System.IO.Path.GetDirectoryName(file);
            List<string> ret = new List<string>();
            string line;
            string pattern = @"^(File)([0-9])+(=)";
            using (var content = await LoadFile(file))
            {
                do
                {
                    line = content.ReadLine();
                    if (line == null) continue;
                    if (Regex.IsMatch(line, pattern)) line = Regex.Replace(line, pattern, "");
                    else continue;
                    if (line.StartsWith("http://") || line.StartsWith("https://"))
                    {
                        ret.Add(line);
                    }
                    else if (line.Contains(":\\") || line.StartsWith("\\\\"))
                    {
                        if (!File.Exists(line)) continue;
                        ret.Add(line);
                    }
                    else
                    {
                        string f = Path.Combine(filedir, line);
                        if (!File.Exists(f)) continue;
                        ret.Add(f);
                    }
                }
                while (line != null);
                return ret;
            }
        }
    }
}
