using LiteDB;
using System.Collections.Generic;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class Notes : ImplementationBase<Note>, INotes
    {
        public Notes(LiteCollection<Note> collection) : base(collection)
        {
        }

        public IEnumerable<Note> GetNotes()
        {
            return EntityCollection.FindAll();
        }

        public void SaveNote(Note note)
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

        public void DeleteNote(string noteName)
        {
            EntityCollection.Delete(n => n.Name == noteName);
        }

        public void SaveNotes(IEnumerable<Note> notes)
        {
            EntityCollection.InsertBulk(notes);
        }

        public void DeleteAll()
        {
            EntityCollection.Delete(x => x.Name != null);
        }
    }
}
