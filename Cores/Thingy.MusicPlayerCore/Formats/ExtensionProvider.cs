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
                ".pls",".m3u",".m3u8",".wpl",".asx"
            };
        }

        private string CombineExtensions(string header, IEnumerable<string> input)
        {
            StringBuilder summed = new StringBuilder();
            StringBuilder individual = new StringBuilder();
            summed.AppendFormat("{0}|", header);
            foreach (var item in input)
            {
                summed.AppendFormat("*{0};", item);
                individual.AppendFormat("|*{0}|*{0};", item);
            }
            summed.Append(individual);
            return summed.ToString();
        }

        public string PlalistsFilterString
        {
            get { return CombineExtensions("Playlists", _PlaylistExtensions); }
        }

        public string AllFormatsFilterString
        {
            get { return CombineExtensions("Supported Files", _StreamExtensions); }
        }

        public IEnumerable<string> AllSupportedFormats
        {
            get
            {
                foreach (var item in _StreamExtensions)
                    yield return item;
                foreach (var item in _PlaylistExtensions)
                    yield return item;
            }
        }

        public string AllFormatsAndPlaylistsFilterString
        {
            get { return CombineExtensions("Supported formats", AllSupportedFormats); }
        }

        public bool IsNetworkStream(string file)
        {
            var lowercase = file.ToLower();
            return lowercase.StartsWith("http://") || lowercase.StartsWith("https://") || lowercase.StartsWith("ftp://");
        }

        public FormatKind GetFormatKind(string file)
        {
            if (string.IsNullOrEmpty(file))
                return FormatKind.Unknown;

            var extension = System.IO.Path.GetExtension(file);

            if (_PlaylistExtensions.Contains(extension))
                return FormatKind.Playlist;
            else if (_StreamExtensions.Contains(extension))
                return FormatKind.Stream;
            else
                return FormatKind.Unknown;
        }

        public bool IsCdStream(string file)
        {
            var lowercase = file.ToLower();
            return lowercase.StartsWith("cd://");
        }
    }
}
