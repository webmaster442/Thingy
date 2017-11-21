using AppLib.Common.Extensions;
using AppLib.WPF.MVVM;
using System.Collections.ObjectModel;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Views;

namespace Thingy.ViewModels
{
    public class ToDoListViewModel: ViewModel
    {
        private IDataBase _db;
        private IApplication _application;

        public ObservableCollection<ToDoItem> Pending { get; private set; }

        public ObservableCollection<ToDoItem> Completed { get; private set; }

        public DelegateCommand AddNewItemCommand { get; private set; }

        public DelegateCommand<int> DeleteItemCommand { get; private set; }


        public ToDoListViewModel(IApplication app, IDataBase db)
        {
            _application = app;
            _db = db;
            Pending = new ObservableCollection<ToDoItem>();
            Completed = new ObservableCollection<ToDoItem>();
            Pending.AddRange(_db.GetUncompletedTasks());
            Completed.AddRange(_db.GetCompletededTasks());
            AddNewItemCommand = DelegateCommand.ToCommand(AddNewItem);
            DeleteItemCommand = DelegateCommand<int>.ToCommand(DeleteItem, CanDelete);
        }

        private void DeleteItem(int obj)
        {
            var item = Pending[obj];
            _db.DeleteToDoItem(item);
            Pending.RemoveAt(obj);
        }

        private bool CanDelete(int obj)
        {
            return obj > -1;
        }

        private void AddNewItem()
        {
            var dialog = new NewToDoItem();
            var item = new ToDoItem();
            dialog.DataContext = item;
            if (_application.ShowDialog(dialog, "New To Do Item") == true)
            {
                Pending.Add(item);
                _db.SaveToDoItem(item);
                
            }

        }
    }
}
