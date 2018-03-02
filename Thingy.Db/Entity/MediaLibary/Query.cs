using LiteDB;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity.MediaLibary
{
    [Serializable]
    public class SongQuery : IEquatable<SongQuery>
    {
        [BsonId]
        public string Name { get; set; }

        [BsonField]
		public StringQuery Artist { get; set; }
        [BsonField]
        public StringQuery Title { get; set; }
        [BsonField]
        public StringQuery Album { get; set; }
        [BsonField]
        public StringQuery Genre { get; set; }
        [BsonField]
        public IntQuery Year { get; set; }
        [BsonField]
        public bool? Liked { get; set; }

		public SongQuery()
        {
            Artist = new StringQuery();
            Title = new StringQuery();
            Album = new StringQuery();
            Genre = new StringQuery();
            Year = new IntQuery();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SongQuery);
        }

        public bool Equals(SongQuery other)
        {
            return other != null &&
                   Name == other.Name &&
                   EqualityComparer<StringQuery>.Default.Equals(Artist, other.Artist) &&
                   EqualityComparer<StringQuery>.Default.Equals(Title, other.Title) &&
                   EqualityComparer<StringQuery>.Default.Equals(Album, other.Album) &&
                   EqualityComparer<StringQuery>.Default.Equals(Genre, other.Genre) &&
                   EqualityComparer<IntQuery>.Default.Equals(Year, other.Year) &&
                   EqualityComparer<bool?>.Default.Equals(Liked, other.Liked);
        }

        public override int GetHashCode()
        {
            var hashCode = -1519485253;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<StringQuery>.Default.GetHashCode(Artist);
            hashCode = hashCode * -1521134295 + EqualityComparer<StringQuery>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<StringQuery>.Default.GetHashCode(Album);
            hashCode = hashCode * -1521134295 + EqualityComparer<StringQuery>.Default.GetHashCode(Genre);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntQuery>.Default.GetHashCode(Year);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Liked);
            return hashCode;
        }

        public static bool operator ==(SongQuery query1, SongQuery query2)
        {
            return EqualityComparer<SongQuery>.Default.Equals(query1, query2);
        }

        public static bool operator !=(SongQuery query1, SongQuery query2)
        {
            return !(query1 == query2);
        }
    }
}
