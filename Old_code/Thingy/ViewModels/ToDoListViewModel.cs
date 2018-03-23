using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppLib.MVVM;
using Thingy.Controls;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Infrastructure;

namespace Thingy.ViewModels
{
    public class ToDoListViewModel : ViewModel, ICanImportExportXMLData
    {
        private IDataBase _db;
        private IApplication _application;

        public TrulyObservableCollection<ToDoItem> Pending { get; set; }

        public TrulyObservableCollection<ToDoItem> Completed { get; set; }

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
            Pending.AddRange(_db.Todo.GetUncompletedTasks());
            Completed.AddRange(_db.Todo.GetCompletededTasks());
            AddNewItemCommand = Command.ToCommand(AddNewItem);
            DeleteItemCommand = Command.ToCommand<int>(DeleteItem, CanDelete);
            DeleteCompletedItemsCommand = Command.ToCommand(DeleteCompletedItems);
        }

        private void Pending_ItemChanged(object sender, ItemChangedEventArgs<ToDoItem> e)
        {
            var item = e.ChangedItem;
            _db.Todo.UpdateToDoItem(item);
            Pending.UpdateCollection(_db.Todo.GetUncompletedTasks());
            Completed.UpdateCollection(_db.Todo.GetCompletededTasks());
        }

        private void DeleteItem(int obj)
        {
            var item = Pending[obj];
            _db.Todo.DeleteToDoItem(item);
            Pending.RemoveAt(obj);
        }

        private bool CanDelete(int obj)
        {
            return obj > -1;
        }

        private async void AddNewItem()
        {
            var dialog = new Views.Dialogs.NewToDoItem();
            var item = new ToDoItem();
            var result = await _application.ShowDialog(dialog, "New To Do Item", item);
            if (result)
            {
                Pending.Add(item);
                _db.Todo.SaveToDoItem(item);

            }
        }

        private void DeleteCompletedItems()
        {
            _db.Todo.DeleteCompletedToDoItems();
            Pending.UpdateCollection(_db.Todo.GetUncompletedTasks());
            Completed.UpdateCollection(_db.Todo.GetCompletededTasks());
        }

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<ToDoItem[]>(xmlData);
                if (append)
                    _db.Todo.SaveToDoItems(import);
                else
                {
                    _db.Todo.DeleteAll();
                    _db.Todo.SaveToDoItems(import);
                }
                App.Current.Dispatcher.Invoke(() =>
                {
                    Pending.Clear();
                    Pending.AddRange(_db.Todo.GetUncompletedTasks());
                    Completed.Clear();
                    Completed.AddRange(_db.Todo.GetCompletededTasks());
                });
            });
        }

        public Task Export(Stream xmlData)
        {
            return Task.Run(() =>
            {
                EntitySerializer.Serialize(xmlData, _db.Todo.GetAllTasks().ToArray());
            });
        }
    }
}
