﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using Thingy.Db.Entity;
using System.Linq;
using System.Windows;

namespace Thingy.Db
{
    public sealed class DataBase : IDataBase, IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;
        private LiteCollection<ToDoItem> _ToDoCollection;
        private LiteCollection<FolderLink> _Folders;

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);
            Files = new DataBaseFileStorage(_db);
            _ToDoCollection = _db.GetCollection<ToDoItem>(nameof(_ToDoCollection));
            _Folders = _db.GetCollection<FolderLink>(nameof(_Folders));
        }

        public IDataBaseFileStorage Files
        {
            get;
            private set;
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

        #region Task management
        public IEnumerable<ToDoItem> GetCompletededTasks()
        {
            return _ToDoCollection.Find(x => x.IsCompleted == true)
                .OrderBy(x => x.DueDate);

        }

        public IEnumerable<ToDoItem> GetUncompletedTasks()
        {
            return _ToDoCollection.Find(x => x.IsCompleted == false);
        }

        public void SaveToDoItem(ToDoItem itemtoSave)
        {
            _ToDoCollection.Insert(itemtoSave);
        }

        public void DeleteToDoItem(ToDoItem toDelete)
        {
            _ToDoCollection.Delete(item => item.Content == toDelete.Content);
        }

        public void UpdateToDoItem(ToDoItem toUpdate)
        {
            var u = (_ToDoCollection.Find(item => item.Content == toUpdate.Content)).FirstOrDefault();
            if (u != null)
            {
                u.CompletedDate = toUpdate.CompletedDate;
                u.Content = toUpdate.Content;
                u.DueDate = toUpdate.DueDate;
                u.IsCompleted = toUpdate.IsCompleted;
                _ToDoCollection.Update(u);
            }
        }

        public void DeleteCompletedToDoItems()
        {
            var q = MessageBox.Show("Delete Completed Items?\nOperation can't be undone.", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _ToDoCollection.Delete(item => item.IsCompleted == true);
            }
        }
        #endregion

        #region Favorite Folders
        public IEnumerable<FolderLink> GetFavoriteFolders()
        {
            return _Folders.FindAll();
        }

        public void SaveFavoriteFolder(FolderLink favorite)
        {
            _Folders.Insert(favorite);
        }

        public void DeleteFavoriteFolder(string foldername)
        {
            _Folders.Delete(folder => folder.Name == foldername);
        }
        #endregion
    }
}
