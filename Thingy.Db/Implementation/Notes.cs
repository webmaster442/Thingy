using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class Notes : ImplementationBase<Note>, IEntityTable<string, Note>
    {
        public Notes(LiteCollection<Note> collection) : base(collection)
        {
        }

        public void Save(Note note)
        {
            var existing = EntityCollection.Find(f => f.Name == note.Name).FirstOrDefault();
            if (existing != null)
            {
                existing.Content = string.Copy(note.Content);
                EntityCollection.Update(existing);
            }
            else
            {
                EntityCollection.Insert(note);
            }
        }

        public void Delete(string noteName)
        {
            EntityCollection.Delete(n => n.Name == noteName);
        }

        public void Save(IEnumerable<Note> notes)
        {
            EntityCollection.InsertBulk(notes);
        }

        public void DeleteAll()
        {
            EntityCollection.Delete(x => x.Name != null);
        }

        public IEnumerable<Note> GetAll()
        {
            return EntityCollection.FindAll();
        }

        public Note GetByKey(string key)
        {
            return EntityCollection.Find(n => n.Name == key).FirstOrDefault();
        }
    }
}
