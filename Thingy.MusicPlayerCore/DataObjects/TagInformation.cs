using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Thingy.MusicPlayerCore.DataObjects
{
    public sealed class TagInformation : IEquatable<TagInformation>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as TagInformation);
        }

        public bool Equals(TagInformation other)
        {
            return other != null &&
                   Title == other.Title &&
                   Artist == other.Artist &&
                   Album == other.Album &&
                   Year == other.Year &&
                   FileName == other.FileName;
        }

        public override int GetHashCode()
        {
            var hashCode = -231015609;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Artist);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Album);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Year);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            return hashCode;
        }

        public static bool operator ==(TagInformation information1, TagInformation information2)
        {
            return EqualityComparer<TagInformation>.Default.Equals(information1, information2);
        }

        public static bool operator !=(TagInformation information1, TagInformation information2)
        {
            return !(information1 == information2);
        }
    }
}
