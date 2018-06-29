using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class VirtualFolders : ImplementationBase<VirtualFolder>, IEntityTable<string, VirtualFolder>
    {
        public VirtualFolders(LiteCollection<VirtualFolder> collection) : base(collection)
        {
        }

        public void Save(VirtualFolder folder)
        {
            var existing = EntityCollection.Find(f => f.Name == folder.Name).FirstOrDefault();
            if (existing != null)
            {
                existing.Files.Clear();
                existing.Files.AddRange(folder.Files);
                EntityCollection.Update(existing);
            }
            else
            {
                EntityCollection.Insert(folder);
            }
        }

        public void Delete(string folderName)
        {
            EntityCollection.Delete(f => f.Name == folderName);
        }

        public VirtualFolder GetByKey(string key)
        {
            return EntityCollection.Find(item => item.Name == key).FirstOrDefault();
        }
    }
}
