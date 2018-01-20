using System.Windows.Media.Imaging;

namespace Thingy.MusicPlayerCore.DataObjects
{
    public sealed class TagInformation
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string FileName { get; set; }
        public BitmapImage Cover { get; set; }

        public static TagInformation Unknown
        {
            get
            {
                return new TagInformation
                {
                    Title = "Unknown",
                    Artist = "Unknown Artist",
                    Album = "Unknown Album",
                    Year = "n/a",
                    FileName = "",
                    Cover = null
                };
            }
        }
    }
}
