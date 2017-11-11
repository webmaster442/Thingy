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
        IEnumerable<ToDoItem> GetCompletedTasks();
        IEnumerable<ToDoItem> GetUncompletedTasks();
    }
}
