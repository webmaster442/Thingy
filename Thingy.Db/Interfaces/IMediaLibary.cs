using System.Collections.Generic;
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
        IEnumerable<RadioStation> GetRadioStations();
        void AddRadioStation(RadioStation station);
    }
}
