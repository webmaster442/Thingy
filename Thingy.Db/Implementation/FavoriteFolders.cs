using LiteDB;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class FavoriteFolders : ImplementationBase<FolderLink>, IEntityTable<string, FolderLink>
    {
        public FavoriteFolders(LiteCollection<FolderLink> collection) : base(collection)
        {
        }

        public void Save(FolderLink favorite)
        {
            EntityCollection.Insert(favorite);
        }

        public void Delete(string foldername)
        {
            EntityCollection.Delete(folder => folder.Name == foldername);
        }

        public FolderLink GetByKey(string key)
        {
            return EntityCollection.Find(folder => folder.Name == key).FirstOrDefault();
        }
    }
}
