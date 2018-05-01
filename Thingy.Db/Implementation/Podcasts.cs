using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;
using Thingy.Db.Interfaces;

namespace Thingy.Db.Implementation
{
    internal class Podcasts: ImplementationBase<PodcastUri>, IPodcasts
    {
        public Podcasts(LiteCollection<PodcastUri> collection) : base(collection)
        {
        }

        public void DeleteAll()
        {
            EntityCollection.Delete(x => x.Name != null);
        }

        public void DeletePodcast(string name)
        {
            EntityCollection.Delete(n => n.Name == name);
        }

        public IEnumerable<PodcastUri> GetPodcasts()
        {
            return EntityCollection.FindAll();
        }

        public void SavePodcast(PodcastUri uri)
        {
            var existing = EntityCollection.Find(f => f.Name == uri.Name).FirstOrDefault();
            if (existing != null)
            {
                existing.Uri = string.Copy(uri.Uri);
                EntityCollection.Update(existing);
            }
            else
            {
                EntityCollection.Insert(uri);
            }
        }

        public void SavePodcasts(IEnumerable<PodcastUri> uris)
        {
            EntityCollection.InsertBulk(uris);
        }
    }
}
