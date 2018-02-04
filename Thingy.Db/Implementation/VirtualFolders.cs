using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class VirtualFolders : ImplementationBase<VirtualFolder>, IVirtualFolders
    {
        public VirtualFolders(LiteCollection<VirtualFolder> collection) : base(collection)
        {
        }

        public IEnumerable<VirtualFolder> GetVirtualFolders()
        {
            return EntityCollection.FindAll();
        }

        public void SaveVirtualFolder(VirtualFolder folder)
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

        public void DeleteVirtualFolder(string folderName)
        {
            EntityCollection.Delete(f => f.Name == folderName);
        }
    }
}
