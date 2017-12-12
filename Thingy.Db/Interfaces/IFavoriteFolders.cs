using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IFavoriteFolders
    {
        IEnumerable<FolderLink> GetFavoriteFolders();
        void SaveFavoriteFolder(FolderLink favorite);
        void DeleteFavoriteFolder(string foldername);
    }
}
