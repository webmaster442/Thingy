using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IDataBase
    {
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
    }
}
