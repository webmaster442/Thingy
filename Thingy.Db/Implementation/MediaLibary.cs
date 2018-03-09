using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Implementation
{
    internal class MediaLibary : ImplementationBase<Song>, IMediaLibary
    {
        private IStoredFiles _files;
        private LiteCollection<RadioStation> _radioStations;
        private LiteCollection<SongQuery> _querydb;
        private MediaLibaryCache _cache;

        public MediaLibary(LiteCollection<Song> collection, 
                           LiteCollection<RadioStation> radios, 
                           LiteCollection<SongQuery> query,
                           IStoredFiles files) : base(collection)
        {
            _files = files;
            _radioStations = radios;
            _querydb = query;
            RestoreCache();
        }

        private void RestoreCache()
        {
            try
            {
                using (var cacheFile = _files.OpenRead(Folders.AlbumsCache, "cache.xml"))
                {
                    var xs = new XmlSerializer(typeof(MediaLibaryCache));
                    _cache = xs.Deserialize(cacheFile) as MediaLibaryCache;
                }
            }
            catch (Exception)
            {
                _cache = new MediaLibaryCache();
            }
        }

        public void AddRadioStation(RadioStation station)
        {
            _radioStations.Insert(station);
        }

        public IEnumerable<RadioStation> GetRadioStations()
        {
            return _radioStations.FindAll();
        }

        public IEnumerable<Song> DoQuery(SongQuery input)
        {
            IEnumerable<Song> matches = null;

            if (input.Artist.HasValue)
            {
                if (matches == null)

                    matches = EntityCollection.Find(song => input.Artist.IsMatch(song.Artist));
                else
                    matches = matches.Where(song => input.Artist.IsMatch(song.Artist));
            }

            if (input.Title.HasValue)
            {
                if (matches == null)
                    matches = EntityCollection.Find(song => input.Title.IsMatch(song.Title));
                else
                    matches = matches.Where(song => input.Title.IsMatch(song.Title));
            }

            if (input.Album.HasValue)
            {
                if (matches == null)
                    matches = EntityCollection.Find(song => input.Album.IsMatch(song.Album));
                else
                    matches = matches.Where(song => input.Album.IsMatch(song.Album));
            }

            if (input.Genre.HasValue)
            {
                if (matches == null)
                    matches = EntityCollection.Find(song => input.Genre.IsMatch(song.Genre));
                else
                    matches = matches.Where(song => input.Genre.IsMatch(song.Genre));
            }

            if (input.Year.HasValue)
            {
                if (matches == null)
                    matches = EntityCollection.Find(song => input.Year.IsMatch(song.Year));
                else
                    matches = matches.Where(song => input.Year.IsMatch(song.Year));
            }

            if (input.Liked.HasValue)
            {
                if (matches == null)
                    matches = EntityCollection.Find(song => song.Liked == input.Liked.Value);
                else
                    matches = matches.Where(song => song.Liked == input.Liked.Value);
            }


            return matches;
        }

        public IEnumerable<string> GetAlbums()
        {
            return _cache.Albums;
        }

        public IEnumerable<string> GetArtists()
        {
            return _cache.Artists;
        }

        public IEnumerable<string> GetGeneires()
        {
            return _cache.Geneires;
        }

        public IEnumerable<string> GetYears()
        {
            return _cache.Years;
        }

        public void AddSong(Song s)
        {
            EntityCollection.Insert(s);
            _cache.SongAdded(s);
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            EntityCollection.InsertBulk(songs);
            _cache.SongsAdded(songs);
        }

        public void SaveCache()
        {
            try
            {
                using (var cacheFile = _files.OpenWrite(Folders.AlbumsCache, "cache.xml"))
                {
                    var xs = new XmlSerializer(typeof(MediaLibaryCache));
                    xs.Serialize(cacheFile, _cache);
                }
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<string> GetQueryNames()
        {
            return _querydb.FindAll().Select(q => q.Name);
        }

        public SongQuery GetQuery(string name)
        {
            return _querydb.Find(q => q.Name == name).FirstOrDefault();
        }

        public void SaveQuery(SongQuery q)
        {
            _querydb.Insert(q);
        }
    }
}
