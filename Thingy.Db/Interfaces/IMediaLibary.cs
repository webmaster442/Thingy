using System.Collections.Generic;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db
{
    public interface IMediaLibary
    {
        IEnumerable<Song> DoQuery(SongQuery input);
        IEnumerable<Song> DoQuery();
        void AddSong(Song s);
        void AddSongs(IEnumerable<Song> songs);
        void DeleteSongs(IEnumerable<Song> songs);
        void DeleteAll();
        void SaveCache();
        IEnumerable<string> GetAlbums();
        IEnumerable<string> GetArtists();
        IEnumerable<string> GetYears();
        IEnumerable<string> GetGeneires();
        IEnumerable<RadioStation> GetRadioStations();
        void AddRadioStation(RadioStation station);
        IEnumerable<string> GetQueryNames();
        SongQuery GetQuery(string name);
        IEnumerable<SongQuery> GetAllQuery();
        void SaveQuery(SongQuery q);
        void SaveQuery(IEnumerable<SongQuery> q);
    }
}
