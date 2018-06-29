using LiteDB;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class Podcasts: ImplementationBase<PodcastUri>, IEntityTable<string, PodcastUri>
    {
        public Podcasts(LiteCollection<PodcastUri> collection) : base(collection)
        {
        }

        public void Delete(string name)
        {
            EntityCollection.Delete(n => n.Name == name);
        }

        public PodcastUri GetByKey(string key)
        {
            return EntityCollection.Find(item => item.Name == key).FirstOrDefault();
        }

        public void Save(PodcastUri uri)
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
    }
}
