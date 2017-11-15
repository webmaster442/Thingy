using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public sealed class DataBase : IDataBase, IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;
        private LiteCollection<ToDoItem> _ToDoCollection;

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);
            _ToDoCollection = _db.GetCollection<ToDoItem>(nameof(_ToDoCollection));
        }

        public void Dispose()
        {
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

        public IEnumerable<ToDoItem> GetCompleteded()
        {
            return _ToDoCollection.Find(x => x.IsCompleted == true);
        }

        public IEnumerable<ToDoItem> GetRelevantItems()
        {
            return _ToDoCollection.Find(x => x.IsCompleted == false);
        }

    }
}
