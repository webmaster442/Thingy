using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity
{
    public class PodcastUri : IEquatable<PodcastUri>
    {
        [BsonId]
        public string Name { get; set; }

        [BsonField]
        [Required]
        public string Uri { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PodcastUri);
        }

        public bool Equals(PodcastUri other)
        {
            return other != null &&
                   Name == other.Name &&
                   Uri == other.Uri;
        }

        public override int GetHashCode()
        {
            var hashCode = -1170516589;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Uri);
            return hashCode;
        }

        public static bool operator ==(PodcastUri uri1, PodcastUri uri2)
        {
            return EqualityComparer<PodcastUri>.Default.Equals(uri1, uri2);
        }

        public static bool operator !=(PodcastUri uri1, PodcastUri uri2)
        {
            return !(uri1 == uri2);
        }
    }
}
