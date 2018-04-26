using LiteDB;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity.MediaLibary
{
    [Serializable]
    public class RadioStation : IEquatable<RadioStation>
    {
        [BsonId]
        public string Name { get; set; }

        [BsonField]
        public string Url { get; set; }

        public RadioStation()
        {

        }

        public RadioStation(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RadioStation);
        }

        public bool Equals(RadioStation other)
        {
            return other != null &&
                   Name == other.Name &&
                   Url == other.Url;
        }

        public override int GetHashCode()
        {
            var hashCode = -1254404684;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Url);
            return hashCode;
        }

        public static bool operator ==(RadioStation station1, RadioStation station2)
        {
            return EqualityComparer<RadioStation>.Default.Equals(station1, station2);
        }

        public static bool operator !=(RadioStation station1, RadioStation station2)
        {
            return !(station1 == station2);
        }
    }
}
