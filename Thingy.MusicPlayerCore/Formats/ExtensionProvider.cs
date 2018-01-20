using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thingy.MusicPlayerCore.Formats
{
    public class ExtensionProvider : IExtensionProvider
    {
        private readonly string[] _TrackerExtensions;
        private readonly string[] _MidiExtensions;
        private readonly string[] _StreamExtensions;
        private readonly string[] _PlaylistExtensions;

        public ExtensionProvider()
        {
            _TrackerExtensions = new string[]
            {
                ".xm",".it",".s3m",".mod",
                ".mtm",".umx",".mo3"
            };
            _MidiExtensions = new string[]
            {
                ".midi",".mid",".rmi",".kar",
            };
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
                foreach (var item in _TrackerExtensions)
                    yield return item;
                foreach (var item in _MidiExtensions)
                    yield return item;
                foreach (var item in _StreamExtensions)
                    yield return item;
            }
        }

        public bool IsMatchForFormat(FormatKind formatKind, string file)
        {
            var extension = System.IO.Path.GetExtension(file);
            switch (formatKind)
            {
                case FormatKind.Midi:
                    return _MidiExtensions.Contains(extension);
                case FormatKind.Playlist:
                    return _PlaylistExtensions.Contains(extension);
                case FormatKind.Stream:
                    return _StreamExtensions.Contains(extension);
                case FormatKind.Tracker:
                    return _StreamExtensions.Contains(extension);
                default:
                    return false;
            }
        }
    }
}
