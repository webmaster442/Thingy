using System;
using System.Collections.Generic;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Implementation
{
    [Serializable]
    public class MediaLibaryCache
    {
        public HashSet<string> Albums { get; }
        public HashSet<string> Artists { get; }
        public HashSet<string> Years { get; }
        public HashSet<string> Geneires { get; }

        public MediaLibaryCache()
        {
            Albums = new HashSet<string>();
            Artists = new HashSet<string>();
            Years = new HashSet<string>();
            Geneires = new HashSet<string>();
        }

        public void SongAdded(Song s)
        {
            Albums.Add(s.Album);
            Artists.Add(s.Artist);
            Years.Add(s.Year.ToString());
            Geneires.Add(s.Genre);
        }

        public void SongsAdded(IEnumerable<Song> songs)
        {
            foreach (var s in songs)
            {
                Albums.Add(s.Album);
                Artists.Add(s.Artist);
                Years.Add(s.Year.ToString());
                Geneires.Add(s.Genre);
            }
        }

        public void Rebuild(IEnumerable<Song> songs)
        {
            Albums.Clear();
            Artists.Clear();
            Years.Clear();
            Geneires.Clear();

            foreach (var s in songs)
            {
                Albums.Add(s.Album);
                Artists.Add(s.Artist);
                Years.Add(s.Year.ToString());
                Geneires.Add(s.Genre);
            }
        }

    }
}
