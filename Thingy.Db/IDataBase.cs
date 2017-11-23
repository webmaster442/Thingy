using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
