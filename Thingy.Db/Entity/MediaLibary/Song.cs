using AppLib.MVVM;
using LiteDB;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity.MediaLibary
{
    [Serializable]
    public class Song: BindableBase, IEquatable<Song>
    {
        private string _filename;
        private string _artist;
        private string _title;
        private string _album;
        private int _year;
        private string _genre;
        private bool _liked;
        private int _track;
        private int _disc;
        private double _length;

        [BsonId]
        public string Filename
        {
            get => _filename;
            set => SetValue(ref _filename, value);
        }

        [BsonField]
        public string Artist
        {
            get => _artist;
            set => SetValue(ref _artist, value);
        }

        [BsonField]
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        [BsonField]
        public string Album
        {
            get => _album;
            set => SetValue(ref _album, value);
        }

        [BsonField]
        public int Year
        {
            get => _year;
            set => SetValue(ref _year, value);
        }

        [BsonField]
        public string Genre
        {
            get => _genre;
            set => SetValue(ref _genre, value);
        }

        [BsonField]
        public bool Liked
        {
            get => _liked;
            set => SetValue(ref _liked, value);
        }

        [BsonField]
        public int Track
        {
            get => _track;
            set => SetValue(ref _track, value);
        }

        [BsonField]
        public int Disc
        {
            get => _disc;
            set => SetValue(ref _disc, value);
        }

        [BsonField]
        public double Length
        {
            get => _length;
            set => SetValue(ref _length, value);
        }

        [BsonField]
        public override bool Equals(object obj)
        {
            return Equals(obj as Song);
        }

        public bool Equals(Song other)
        {
            return other != null &&
                   _filename == other._filename &&
                   _artist == other._artist &&
                   _title == other._title &&
                   _album == other._album &&
                   _year == other._year &&
                   _genre == other._genre &&
                   _liked == other._liked &&
                   _track == other._track &&
                   _disc == other._disc &&
                   _length == other._length;
        }

        public override int GetHashCode()
        {
            var hashCode = 131465599;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_filename);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_artist);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_album);
            hashCode = hashCode * -1521134295 + _year.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_genre);
            hashCode = hashCode * -1521134295 + _liked.GetHashCode();
            hashCode = hashCode * -1521134295 + _track.GetHashCode();
            hashCode = hashCode * -1521134295 + _disc.GetHashCode();
            hashCode = hashCode * -1521134295 + _length.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Song song1, Song song2)
        {
            return EqualityComparer<Song>.Default.Equals(song1, song2);
        }

        public static bool operator !=(Song song1, Song song2)
        {
            return !(song1 == song2);
        }
    }
}
