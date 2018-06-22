using AppLib.MVVM;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.CoreModules;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.CoreModules.ViewModels
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
            AddNewItemCommand = Command.CreateCommand(AddNewItem);
            DeleteItemCommand = Command.CreateCommand<int>(DeleteItem, CanDelete);
            DeleteCompletedItemsCommand = Command.CreateCommand(DeleteCompletedItems);
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
            var dialog = new CoreModules.Dialogs.NewToDoItem();
            var item = new ToDoItem();
            var result = await _application.ShowDialog("New To Do Item", dialog, DialogButtons.OkCancel, true, item);
            if (result)
            {
                Pending.Add(item);
                _db.Todo.SaveToDoItem(item);

            }
        }

        private async void DeleteCompletedItems()
        {
            var result = await _application.ShowMessageBox("Question", "Delete Completed Items?\nOperation can't be undone.", DialogButtons.YesNo);
            if (result)
            {
                _db.Todo.DeleteCompletedToDoItems();
                Pending.UpdateCollection(_db.Todo.GetUncompletedTasks());
                Completed.UpdateCollection(_db.Todo.GetCompletededTasks());
            }
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
                _application.CurrentDispatcher.Invoke(() =>
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
