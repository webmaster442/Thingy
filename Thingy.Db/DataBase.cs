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
                                   INotes,
                                   IAlarms,
                                   IDisposable
    {
        private LiteDatabase _db;
        private Stream _filestream;
        private LiteCollection<ToDoItem> _ToDoCollection;
        private LiteCollection<FolderLink> _FolderLinkCollection;
        private LiteCollection<VirtualFolder> _VirtualFolderCollection;
        private LiteCollection<LauncherProgram> _ProgramsCollection;
        private LiteCollection<Note> _NotesCollection;
        private LiteCollection<Alarm> _AlarmsCollection;

        public DataBase(string file)
        {
            _filestream = File.Open(file, System.IO.FileMode.OpenOrCreate);
            _db = new LiteDatabase(_filestream);
            _ToDoCollection = _db.GetCollection<ToDoItem>(nameof(_ToDoCollection));
            _FolderLinkCollection = _db.GetCollection<FolderLink>(nameof(_FolderLinkCollection));
            _VirtualFolderCollection = _db.GetCollection<VirtualFolder>(nameof(_VirtualFolderCollection));
            _ProgramsCollection = _db.GetCollection<LauncherProgram>(nameof(_ProgramsCollection));
            _NotesCollection = _db.GetCollection<Note>(nameof(_NotesCollection));
            _AlarmsCollection = _db.GetCollection<Alarm>(nameof(_AlarmsCollection));
            _ProgramsCollection.EnsureIndex(p => p.Path);
        }

        #region IDatabase Implementation

        public ITodo Todo => this;

        public IFavoriteFolders FavoriteFolders => this;

        public IVirtualFolders VirtualFolders => this;

        public IPrograms Programs => this;

        public INotes Notes => this;

        public IAlarms Alarms => this;
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
            return _ProgramsCollection.FindAll();
        }

        public void SaveLauncherProgram(LauncherProgram program)
        {
            _ProgramsCollection.Insert(program);
        }

        public void DeleteLauncherProgram(string name)
        {
            _ProgramsCollection.Delete(p => p.Name == name);
        }

        public void UpdateLauncherProgramByName(string oldname, LauncherProgram newdata)
        {
            var existing = _ProgramsCollection.Find(f => f.Name == oldname).FirstOrDefault();
            if (existing != null)
            {
                _ProgramsCollection.Delete(p => p == existing);
                _ProgramsCollection.Insert(newdata);
            }
        }
        #endregion

        #region Notes
        public IEnumerable<Note> GetNotes()
        {
            return _NotesCollection.FindAll();
        }

        public void SaveNote(Note note)
        {
            var existing = _NotesCollection.Find(f => f.Name == note.Name).FirstOrDefault();
            if (existing != null)
            {
                existing.Content = string.Copy(note.Content);
                _NotesCollection.Update(existing);
            }
            else
            {
                _NotesCollection.Insert(note);
            }
        }

        public void DeleteNote(string noteName)
        {
            _NotesCollection.Delete(n => n.Name == noteName);
        }
        #endregion

        #region Alarms
        public IEnumerable<Alarm> GetAlarms()
        {
            return _AlarmsCollection.FindAll();
        }

        public IEnumerable<Alarm> GetActiveAlarms()
        {
            return _AlarmsCollection.Find(alarm => alarm.Active == true);
        }
        #endregion
    }
}
