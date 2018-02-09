using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface ITodo
    {
        IEnumerable<ToDoItem> GetUncompletedTasks();
        IEnumerable<ToDoItem> GetCompletededTasks();
        IEnumerable<ToDoItem> GetAllTasks();
        void SaveToDoItem(ToDoItem itemtoSave);
        void SaveToDoItems(IEnumerable<ToDoItem> itemtoSave);
        void DeleteToDoItem(ToDoItem toDelete);
        void UpdateToDoItem(ToDoItem item);
        void DeleteCompletedToDoItems();
        void DeleteAll();
    }
}
