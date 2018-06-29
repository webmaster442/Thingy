using LiteDB;
using System;
using System.IO;
using Thingy.Db.Entity;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db
{
    public sealed class DataBase : IDataBase,
                                   IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;

        public ITodo Todo { get; private set; }

        public IFavoriteFolders FavoriteFolders { get; private set; }

        public IEntityTable<string, VirtualFolder> VirtualFolders { get; private set; }

        public IPrograms Programs { get; private set; }

        public IEntityTable<string, Note> Notes { get; private set; }

        public IAlarms Alarms { get; private set; }

        public IStoredFiles StoredFiles { get; private set; }

        public IMediaLibary MediaLibary { get; private set; }

        public IPodcasts Podcasts { get; private set; }

        public DataBase(string file)
        {
            bool needsInit = false;
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);

            if (_filestream.Length == 0)
            {
                needsInit = true;
            }

            _db = new LiteDatabase(_filestream);

            Todo = new Implementation.ToDo(_db.GetCollection<ToDoItem>(CollectionNames.Todo));

            FavoriteFolders = new Implementation.FavoriteFolders(_db.GetCollection<FolderLink>(CollectionNames.FavoriteFolders));

            VirtualFolders = new Implementation.VirtualFolders(_db.GetCollection<VirtualFolder>(CollectionNames.VirtualFolders));

            Programs = new Implementation.Programs(_db.GetCollection<LauncherProgram>(CollectionNames.Programs));

            Notes = new Implementation.Notes(_db.GetCollection<Note>(CollectionNames.Notes));

            Alarms = new Implementation.Alarms(_db.GetCollection<Alarm>(CollectionNames.Alarms));
            StoredFiles = new Implementation.StoredFiles(_db);

            MediaLibary = new Implementation.MediaLibary(_db.GetCollection<Song>(CollectionNames.Songs), 
                                                         _db.GetCollection<RadioStation>(CollectionNames.Radios),
                                                         _db.GetCollection<SongQuery>(CollectionNames.Queries),
                                                         StoredFiles);

            Podcasts = new Implementation.Podcasts(_db.GetCollection<PodcastUri>(CollectionNames.Podcasts));

            if (needsInit)
            {
                DataBaseInitializer.Init(this);
            }
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
