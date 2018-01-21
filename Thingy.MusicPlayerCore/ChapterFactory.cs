using Mp4Chapters;
using System;
using System.Collections.Generic;
using System.IO;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore
{
    public static class ChapterFactory
    {
        private const double Minute = 60.0d;

        public static IEnumerable<Chapter> GetChapters(string filename, double parsedlength)
        {
            var extension = System.IO.Path.GetExtension(filename).ToLower();
            List<Chapter> ret = new List<Chapter>();
            switch (extension)
            {
                case ".mp3":
                    TagLib.File f = TagLib.File.Create(filename);
                    var mp3Chapters = CreateChaptersAudibleFormat(f.Tag.Lyrics);
                    if (mp3Chapters.Count > 0) ret.AddRange(mp3Chapters);
                    break;
                case ".mp4":
                case ".m4a":
                case ".m4b":
                    var mp4Chapters = CreateChaptersMpeg4(filename);
                    if (mp4Chapters.Count > 0) ret.AddRange(mp4Chapters);
                    break;
            }

            if (ret.Count == 0)
            {
                var byLength = CreateChaptersByLength(parsedlength);
                ret.AddRange(byLength);
            }

            return ret;
        }

        private static List<Chapter> CreateChaptersByLength(double parsedLength)
        {
            List<Chapter> ret = new List<Chapter>();

            var divider = Minute;
            if (parsedLength < 10 * Minute)
            {
                divider = Minute;
            }
            else if (parsedLength >= 10 * Minute)
            {
                divider = 5.0 * Minute;
            }
            double position = 0;
            ret.Add(new Chapter
            {
                Title = "Jump to Begining",
                Position = 0
            });
            while (position + divider < parsedLength)
            {
                position += divider;
                ret.Add(new Chapter
                {
                    Title = $"Jump to: {TimeSpan.FromSeconds(position)}",
                    Position = position
                });
            }
            return ret;
        }

        private static List<Chapter> CreateChaptersMpeg4(string filename)
        {
            List<Chapter> ret = new List<Chapter>();
            using (var stream = File.OpenRead(filename))
            {
                var extractor = new ChapterExtractor(new StreamWrapper(stream));
                extractor.Run();
                foreach (var chapter in extractor.Chapters)
                {
                    ret.Add(new Chapter
                    {
                        Title = chapter.Name,
                        Position = chapter.Time.TotalSeconds
                    });
                }
            }
            return ret;
        }

        private static List<Chapter> CreateChaptersAudibleFormat(string lyrics)
        {
            List<Chapter> ret = new List<Chapter>();

            int count = 0;

            if (!string.IsNullOrEmpty(lyrics))
            {
                using (var sr = new StringReader(lyrics))
                {
                    string line = null;
                    do
                    {
                        line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;

                        var parts = line.Split(' ');
                        TimeSpan ts;
                        if (TimeSpan.TryParse(parts[0], out ts))
                        {
                            ++count;
                            if (parts.Length > 0)
                            {
                                ret.Add(new Chapter
                                {
                                    Title = line.Replace(parts[0], ""),
                                    Position = ts.TotalSeconds
                                });
                            }
                            else
                            {
                                ret.Add(new Chapter
                                {
                                    Title = String.Format("Chapter {0}", count),
                                    Position = ts.TotalSeconds
                                });
                            }
                        }
                    }
                    while (line != null);
                }
            }
            return ret;
        }
    }
}
