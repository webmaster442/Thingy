using AppLib.MVVM;
using Thingy.Controls;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Views;

namespace Thingy.ViewModels
{
    public class ToDoListViewModel: ViewModel
    {
        private IDataBase _db;
        private IApplication _application;

        public TrulyObservableCollection<ToDoItem> Pending { get;  set; }

        public TrulyObservableCollection<ToDoItem> Completed { get;  set; }

        public DelegateCommand AddNewItemCommand { get; private set; }

        public DelegateCommand<int> DeleteItemCommand { get; private set; }

        public DelegateCommand DeleteCompletedItemsCommand { get; private set; }


        public ToDoListViewModel(IApplication app, IDataBase db)
        {
            _application = app;
            _db = db;
            Pending = new TrulyObservableCollection<ToDoItem>();
            Pending.ItemChanged += Pending_ItemChanged;
            Completed = new TrulyObservableCollection<ToDoItem>();
            Pending.AddRange(_db.GetUncompletedTasks());
            Completed.AddRange(_db.GetCompletededTasks());
            AddNewItemCommand = Command.ToCommand(AddNewItem);
            DeleteItemCommand = Command.ToCommand<int>(DeleteItem, CanDelete);
            DeleteCompletedItemsCommand = Command.ToCommand(DeleteCompletedItems);
        }

        private void Pending_ItemChanged(object sender, ItemChangedEventArgs<ToDoItem> e)
        {
            var item = e.ChangedItem;
            _db.UpdateToDoItem(item);
            Pending.UpdateCollection(_db.GetUncompletedTasks());
            Completed.UpdateCollection(_db.GetCompletededTasks());
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
            if (_application.ShowDialog(dialog, "New To Do Item", item) == true)
            {
                Pending.Add(item);
                _db.SaveToDoItem(item);
                
            }
        }

        private void DeleteCompletedItems()
        {
            _db.DeleteCompletedToDoItems();
            Pending.UpdateCollection(_db.GetUncompletedTasks());
            Completed.UpdateCollection(_db.GetCompletededTasks());
        }
    }
}
