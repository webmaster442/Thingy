using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Factories
{
    public static class QueryFactory
    {
        public static SongQuery ArtistQuery(string artist)
        {
            return new SongQuery
            {
                Artist = new StringQuery(artist, StringOperator.Exactmatch)
            };
        }

        public static SongQuery AlbumQuery(string album)
        {
            return new SongQuery
            {
                Album = new StringQuery(album, StringOperator.Exactmatch)
            };
        }

        public static SongQuery YearQuery(int year)
        {
            return new SongQuery
            {
                Year = new IntQuery(year, IntOperator.Equals)
            };
        }

        public static SongQuery GenreQuery(string genre)
        {
            return new SongQuery
            {
                Genre = new StringQuery(genre, StringOperator.Exactmatch)
            };
        }
    }
}
