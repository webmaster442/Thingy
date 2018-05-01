using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IPodcasts
    {
        IEnumerable<PodcastUri> GetPodcasts();
        void SavePodcast(PodcastUri uri);
        void SavePodcasts(IEnumerable<PodcastUri> uris);
        void DeletePodcast(string name);
        void DeleteAll();
    }
}