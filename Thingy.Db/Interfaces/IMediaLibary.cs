using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.Interfaces
{
    public interface IMediaLibary
    {
        IEnumerable<Song> DoQuery(Query input);
        IEnumerable<string> GetAlbums();
        IEnumerable<string> GetArtists();
        IEnumerable<int> GetYears();
        IEnumerable<string> GetGeneires();
    }
}
