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
        private LiteCollection<ToDoItem> ToDoCollection;

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);
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

        public IEnumerable<ToDoItem> GetCompletedTasks()
        {
            return ToDoCollection.Find(q => q.IsCompleted == true);
        }

        public IEnumerable<ToDoItem> GetUncompletedTasks()
        {
            return ToDoCollection.Find(q => q.IsCompleted == false);
        }
    }
}
