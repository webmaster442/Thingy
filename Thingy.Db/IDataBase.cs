using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IDataBase
    {
        IDataBaseFileStorage Files { get; }
        ITodo Todo { get; }
        IFavoriteFolders FavoriteFolders { get; }
        IVirtualFolders VirtualFolders { get; }
        IPrograms Programs { get; }
    }
}
