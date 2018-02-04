using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Implementation
{
    internal class MediaLibary : ImplementationBase<Song>, IMediaLibary
    {
        private IStoredFiles _files;

        public MediaLibary(LiteCollection<Song> collection, IStoredFiles files) : base(collection)
        {
            _files = files;
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
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetArtists()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetGeneires()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetYears()
        {
            throw new NotImplementedException();
        }
    }
}
