using LiteDB;
using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class FavoriteFolders : ImplementationBase<FolderLink>, IFavoriteFolders
    {
        public FavoriteFolders(LiteCollection<FolderLink> collection) : base(collection)
        {
        }

        public IEnumerable<FolderLink> GetFavoriteFolders()
        {
            return EntityCollection.FindAll();
        }

        public void SaveFavoriteFolder(FolderLink favorite)
        {
            EntityCollection.Insert(favorite);
        }

        public void DeleteFavoriteFolder(string foldername)
        {
            EntityCollection.Delete(folder => folder.Name == foldername);
        }
    }
}
