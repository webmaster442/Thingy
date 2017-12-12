using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLib.MVVM;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class ProgramsViewModel: ViewModel
    {
        private IDataBase _db;

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<LauncherProgram> EditCommand { get; private set; }
        public DelegateCommand<LauncherProgram> DeleteCommand { get; private set; }

        public ObservableCollection<LauncherProgram> Programs { get; private set; }

        public ProgramsViewModel(IDataBase db)
        {
            _db = db;
            Programs = new ObservableCollection<LauncherProgram>();
            AddCommand = Command.ToCommand(Add);
            EditCommand = Command.ToCommand<LauncherProgram>(Edit, CanEditOrDelete);
            DeleteCommand = Command.ToCommand<LauncherProgram>(Delete, CanEditOrDelete);
        }

        private bool CanEditOrDelete(LauncherProgram obj)
        {
            return obj != null;
        }

        private void Delete(LauncherProgram obj)
        {
            _db.Programs.DeleteLauncherProgram(obj.Name);
        }

        private void Edit(LauncherProgram obj)
        {
            throw new NotImplementedException();
        }

        private void Add()
        {
            throw new NotImplementedException();
        }
    }
}
