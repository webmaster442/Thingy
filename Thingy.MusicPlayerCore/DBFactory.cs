using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.MusicPlayerCore
{
    public static class DBFactory
    {
        public static Song CreateSong(string file)
        {
            try
            {
                TagLib.File tags = TagLib.File.Create(file);
                var artist = "";
                if (tags.Tag.Performers != null && tags.Tag.Performers.Length != 0)
                {
                    artist = tags.Tag.Performers[0];
                }
                return new Song
                {
                    Year = Convert.ToInt32(tags.Tag.Year),
                    Album = tags.Tag.Album,
                    Title = tags.Tag.Title,
                    Artist = artist,
                    Filename = file,
                    Genre = tags.Tag.Genres[0],
                    Length = tags.Length,
                    Disc = Convert.ToInt32(tags.Tag.Disc),
                    Track = Convert.ToInt32(tags.Tag.Track)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Task<List<Song>> CreateSongs(string[] files)
        {
            return Task.Run(() =>
            {
                var ret = new List<Song>(files.Length);
                foreach (var file in files)
                {
                    ret.Add(CreateSong(file));
                }
                return ret;
            });
        }

        public static MemoryStream GetCover(string fileName)
        {
            MemoryStream ret = new MemoryStream();
            try
            {
                TagLib.File tags = TagLib.File.Create(fileName);
                if (tags.Tag.Pictures.Length > 0)
                {
                    var picture = tags.Tag.Pictures[0].Data;
                    using (var ms = new MemoryStream(picture.Data))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.DecodePixelWidth = 300;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();

                        var enc = new JpegBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(bitmap));
                        enc.QualityLevel = 90;
                        enc.Save(ret);
                    }
                }
            }
            catch (Exception)
            {

            }
            return ret;
        }
    }
}
