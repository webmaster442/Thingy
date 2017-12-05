using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IDataBase
    {
        IDataBaseFileStorage Files { get; }
        //---------------------------------------------
        IEnumerable<ToDoItem> GetUncompletedTasks();
        IEnumerable<ToDoItem> GetCompletededTasks();
        void SaveToDoItem(ToDoItem itemtoSave);
        void DeleteToDoItem(ToDoItem toDelete);
        void UpdateToDoItem(ToDoItem item);
        void DeleteCompletedToDoItems();
        //---------------------------------------------
        IEnumerable<FolderLink> GetFavoriteFolders();
        void SaveFavoriteFolder(FolderLink favorite);
        void DeleteFavoriteFolder(string foldername);
        //---------------------------------------------
        IEnumerable<VirualFolder> GetVirtualFolders();
        void SaveVirtualFolder(VirualFolder folder);
        void DeleteVirtualFolder(string folderName);
    }
}
