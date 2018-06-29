using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class ToDo : ImplementationBase<ToDoItem>, ITodo
    {
        public ToDo(LiteCollection<ToDoItem> collection) : base(collection)
        {
        }

        public IEnumerable<ToDoItem> GetCompleteded()
        {
            return EntityCollection.Find(x => x.IsCompleted == true)
                .OrderBy(x => x.DueDate);

        }

        public IEnumerable<ToDoItem> GetUncompleted()
        {
            return EntityCollection.Find(x => x.IsCompleted == false);
        }

        public void Save(ToDoItem itemtoSave)
        {
            EntityCollection.Insert(itemtoSave);
        }

        public void Delete(ToDoItem toDelete)
        {
            EntityCollection.Delete(item => item.Name == toDelete.Name);
        }

        public void UpdateToDoItem(ToDoItem toUpdate)
        {
            var u = (EntityCollection.Find(item => item.Name == toUpdate.Name)).FirstOrDefault();
            if (u != null)
            {
                u.CompletedDate = toUpdate.CompletedDate;
                u.Name = toUpdate.Name;
                u.DueDate = toUpdate.DueDate;
                u.IsCompleted = toUpdate.IsCompleted;
                EntityCollection.Update(u);
            }
        }

        public void DeleteCompletedToDoItems()
        {
            EntityCollection.Delete(item => item.IsCompleted == true);
        }

        public ToDoItem GetByKey(string key)
        {
            return EntityCollection.Find(item => item.Name == key).FirstOrDefault();
        }

        public void Delete(string key)
        {
            EntityCollection.Delete(item => item.Name == key);
        }
    }
}
