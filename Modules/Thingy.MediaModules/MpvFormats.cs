using System;
using System.Collections.Generic;
using System.Linq;

namespace Thingy.MediaModules
{
    internal static class Formats
    {
        public static IEnumerable<string> SupportedVideoFormats
        {
            get
            {
                yield return ".wmv";
                yield return ".asf";
                yield return ".mpeg";
                yield return ".mpg";
                yield return ".avi";
                yield return ".vob";
                yield return ".mkv";
                yield return ".mp4";
                yield return ".3gp";
                yield return ".flv";
                yield return ".f4v";
            }
        }

        public static IEnumerable<string> SupportedAudioFormats
        {
            get
            {
                yield return ".mp1";
                yield return ".mp2";
                yield return ".mp3";
                yield return ".m4a";
                yield return ".m4b";
                yield return ".aac";
                yield return ".flac";
                yield return ".ac3";
                yield return ".wv";
                yield return ".wav";
                yield return ".wma";
                yield return ".ogg";
                yield return ".ape";
                yield return ".mpc";
                yield return ".mp+";
                yield return ".mpp";
                yield return ".ofr";
                yield return ".ofs";
                yield return ".spx";
                yield return ".tta";
                yield return ".dsf";
                yield return ".dsdiff";
                yield return ".opus";
            }
        }

        internal static bool IsYoutubeUrl(string pathOrExtension)
        {
            var YoutubePrefixes = new string[]
            {
                "https://www.youtube.com/watch?v=",
                "https://youtu.be/",
                "http://www.youtube.com/watch?v=",
                "http://youtu.be/",
            };

            return YoutubePrefixes.Contains(pathOrExtension);
        }
    }
}
