using AppLib.Common.Extensions;
using AppLib.WPF.MVVM;
using System.Collections.ObjectModel;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class ToDoListViewModel: ViewModel
    {
        private IDataBase _db;

        public ToDoListViewModel(IDataBase db)
        {
            _db = db;
            Pending = new ObservableCollection<ToDoItem>();
            Completed = new ObservableCollection<ToDoItem>();
            Pending.AddRange(_db.GetRelevantItems());
            Completed.AddRange(_db.GetCompleteded());
        }

        public ObservableCollection<ToDoItem> Pending { get; private set; }

        public ObservableCollection<ToDoItem> Completed { get; private set; }
    }
}
