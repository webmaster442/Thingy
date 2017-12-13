using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using Thingy.Db.Entity;
using System.Linq;
using System.Windows;

namespace Thingy.Db
{
    public sealed class DataBase : IDataBase, 
                                   IVirtualFolders,
                                   ITodo,
                                   IFavoriteFolders,
                                   IPrograms, 
                                   IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;
        private LiteCollection<ToDoItem> _ToDoCollection;
        private LiteCollection<FolderLink> _FolderLinkCollection;
        private LiteCollection<VirtualFolder> _VirtualFolderCollection;
        private LiteCollection<LauncherProgram> _Programs;

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);
            _ToDoCollection = _db.GetCollection<ToDoItem>(nameof(_ToDoCollection));
            _FolderLinkCollection = _db.GetCollection<FolderLink>(nameof(_FolderLinkCollection));
            _VirtualFolderCollection = _db.GetCollection<VirtualFolder>(nameof(_VirtualFolderCollection));
            _Programs = _db.GetCollection<LauncherProgram>(nameof(_Programs));
            _Programs.EnsureIndex(p => p.Path);
        }

        #region IDatabase Implementation

        public ITodo Todo => this;

        public IFavoriteFolders FavoriteFolders => this;

        public IVirtualFolders VirtualFolders => this;

        public IPrograms Programs => this;

        #endregion

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
            return _FolderLinkCollection.FindAll();
        }

        public void SaveFavoriteFolder(FolderLink favorite)
        {
            _FolderLinkCollection.Insert(favorite);
        }

        public void DeleteFavoriteFolder(string foldername)
        {
            _FolderLinkCollection.Delete(folder => folder.Name == foldername);
        }
        #endregion

        #region Virtual Folders
        public IEnumerable<VirtualFolder> GetVirtualFolders()
        {
            return _VirtualFolderCollection.FindAll();
        }

        public void SaveVirtualFolder(VirtualFolder folder)
        {
            var existing = _VirtualFolderCollection.Find(f => f.Name == folder.Name).FirstOrDefault();
            if (existing != null)
            {
                existing.Files.Clear();
                existing.Files.AddRange(folder.Files);
                _VirtualFolderCollection.Update(existing);
            }
            else
            {
                _VirtualFolderCollection.Insert(folder);
            }
        }

        public void DeleteVirtualFolder(string folderName)
        {
            _VirtualFolderCollection.Delete(f => f.Name == folderName);
        }
        #endregion

        #region Program launcher
        public IEnumerable<LauncherProgram> GetPrograms()
        {
            return _Programs.FindAll();
        }

        public void SaveLauncherProgram(LauncherProgram program)
        {

                _Programs.Insert(program);
        }

        public void DeleteLauncherProgram(string name)
        {
            _Programs.Delete(p => p.Name == name);
        }

        public void UpdateLauncherProgramByName(string oldname, LauncherProgram newdata)
        {
            var existing = _Programs.Find(f => f.Name == oldname).FirstOrDefault();
            if (existing != null)
            {
                _Programs.Delete(p => p == existing);
                _Programs.Insert(newdata);
            }
        }
        #endregion
    }
}
