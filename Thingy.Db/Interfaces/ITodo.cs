using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface ITodo
    {
        IEnumerable<ToDoItem> GetUncompletedTasks();
        IEnumerable<ToDoItem> GetCompletededTasks();
        void SaveToDoItem(ToDoItem itemtoSave);
        void DeleteToDoItem(ToDoItem toDelete);
        void UpdateToDoItem(ToDoItem item);
        void DeleteCompletedToDoItems();
    }
}
