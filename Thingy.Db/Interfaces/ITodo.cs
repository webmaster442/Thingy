using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface ITodo: IEntityTable<string, ToDoItem>
    {
        IEnumerable<ToDoItem> GetUncompleted();
        IEnumerable<ToDoItem> GetCompleteded();
        void Delete(ToDoItem toDelete);
        void UpdateToDoItem(ToDoItem item);
        void DeleteCompletedToDoItems();
    }
}
