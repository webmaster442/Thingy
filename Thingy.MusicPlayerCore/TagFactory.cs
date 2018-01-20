using System;
using System.Windows.Media.Imaging;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore
{
    public static class TagFactory
    {
        public static TagInformation CreateTagInfoFromFile(string fileName)
        {
            try
            {
                TagLib.File tags = TagLib.File.Create(fileName);
                var year = tags.Tag.Year.ToString();
                var album = tags.Tag.Album;
                var title = tags.Tag.Title;
                var artist = "";
                if (tags.Tag.Performers != null && tags.Tag.Performers.Length != 0)
                {
                    artist = tags.Tag.Performers[0];
                }
                return new TagInformation
                {
                    Artist = artist,
                    Album = album,
                    Cover = GetCover(tags),
                    Title = title,
                    Year = year,
                    FileName = System.IO.Path.GetFileName(fileName)
                };

            }
            catch (Exception)
            {
                var unknown = TagInformation.Unknown;
                unknown.FileName = System.IO.Path.GetFileName(fileName);
                return unknown;
            }
        }

        private static BitmapImage GetCover(TagLib.File tags)
        {
            if (tags.Tag.Pictures.Length > 0)
            {
                var picture = tags.Tag.Pictures[0].Data;
                using (var ms = new System.IO.MemoryStream(picture.Data))
                {
                    BitmapImage ret = new BitmapImage();
                    ret.BeginInit();
                    ret.StreamSource = ms;
                    ret.DecodePixelWidth = 300;
                    ret.CacheOption = BitmapCacheOption.OnLoad;
                    ret.EndInit();
                    return ret;
                }
            }
            return null;
        }
    }
}