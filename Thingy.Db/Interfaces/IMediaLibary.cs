using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db
{
    public interface IMediaLibary
    {
        IEnumerable<Song> DoQuery(SongQuery input);
        IEnumerable<string> GetAlbums();
        IEnumerable<string> GetArtists();
        IEnumerable<int> GetYears();
        IEnumerable<string> GetGeneires();
    }
}
