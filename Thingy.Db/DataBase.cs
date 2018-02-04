using LiteDB;
using System;
using System.IO;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public sealed class DataBase : IDataBase,
                                   IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;

        public ITodo Todo { get; private set; }

        public IFavoriteFolders FavoriteFolders { get; private set; }

        public IVirtualFolders VirtualFolders { get; private set; }

        public IPrograms Programs { get; private set; }

        public INotes Notes { get; private set; }

        public IAlarms Alarms { get; private set; }

        public IStoredFiles StoredFiles { get; private set; }

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);

            Todo = new Implementation.ToDo(_db.GetCollection<ToDoItem>(CollectionNames.Todo));
            FavoriteFolders = new Implementation.FavoriteFolders(_db.GetCollection<FolderLink>(CollectionNames.FavoriteFolders));
            VirtualFolders = new Implementation.VirtualFolders(_db.GetCollection<VirtualFolder>(CollectionNames.VirtualFolders));
            Programs = new Implementation.Programs(_db.GetCollection<LauncherProgram>(CollectionNames.Programs));
            Notes = new Implementation.Notes(_db.GetCollection<Note>(CollectionNames.Notes));
            Alarms = new Implementation.Alarms(_db.GetCollection<Alarm>(CollectionNames.Alarms));
            StoredFiles = new Implementation.StoredFiles(_db);
        }

        public void Dispose()
        {
            if (StoredFiles != null)
            {
                StoredFiles = null;
            }
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            if (_filestream != null)
            {
                _filestream.Dispose();
                _filestream = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
