using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface INotes
    {
        IEnumerable<Note> GetNotes();
        void SaveNote(Note note);
        void DeleteNote(string noteName);
    }
}
