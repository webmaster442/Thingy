using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Factories
{
    public class QueryFactory
    {
        public SongQuery ArtistQuery(string artist)
        {
            return new SongQuery
            {
                Artist = new StringQuery(artist, StringOperator.Exactmatch)
            };
        }

        public SongQuery AlbumQuery(string album)
        {
            return new SongQuery
            {
                Album = new StringQuery(album, StringOperator.Exactmatch)
            };
        }
    }
}
