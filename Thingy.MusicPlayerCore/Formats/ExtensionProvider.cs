using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thingy.MusicPlayerCore.Formats
{
    public class ExtensionProvider : IExtensionProvider
    {
        private readonly string[] _StreamExtensions;
        private readonly string[] _PlaylistExtensions;

        public ExtensionProvider()
        {
            _StreamExtensions = new string[]
            {
                ".mp1",".mp2",".mp3",".mp4",
                ".m4a",".m4b",".aac",".flac",
                ".ac3",".wv",".wav",".wma",".asf",
                ".ogg",".ape",".mpc",".mp+",".mpp",
                ".ofr",".ofs",".spx",".tta",
                ".dsf",".dsdiff",".opus"
            };
            _PlaylistExtensions = new string[]
            {
                ".pls",".m3u",".wpl",".asx"
            };
        }

        private string CombineExtensions(string header, IEnumerable<string> input)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}|");
            foreach (var item in input)
            {
                sb.AppendFormat("*{0};", item);
            }
            return sb.ToString();
        }

        public string PlalistsFilterString
        {
            get { return CombineExtensions("Playlists", _PlaylistExtensions); }
        }

        public string AllFormatsFilterString
        {
            get { return CombineExtensions("Supported Files", AllSupportedFormats); }
        }

        public IEnumerable<string> AllSupportedFormats
        {
            get
            {
                foreach (var item in _StreamExtensions)
                    yield return item;
            }
        }

        public bool IsMatchForFormat(FormatKind formatKind, string file)
        {
            var extension = System.IO.Path.GetExtension(file);
            switch (formatKind)
            {
                case FormatKind.Playlist:
                    return _PlaylistExtensions.Contains(extension);
                case FormatKind.Stream:
                    return _StreamExtensions.Contains(extension);
                default:
                    return false;
            }
        }

        public bool IsNetworkStream(string file)
        {
            var lowercase = file.ToLower();
            return lowercase.StartsWith("http://") || lowercase.StartsWith("https://") || lowercase.StartsWith("ftp://");
        }
    }
}
