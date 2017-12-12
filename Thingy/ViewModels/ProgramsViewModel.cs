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
        private IApplication _application;

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<LauncherProgram> EditCommand { get; private set; }
        public DelegateCommand<LauncherProgram> DeleteCommand { get; private set; }
        public DelegateCommand RunCommand { get; private set; }

        public ObservableCollection<LauncherProgram> Programs { get; private set; }

        public ProgramsViewModel(IApplication app, IDataBase db)
        {
            _db = db;
            _application = app;
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
            var dialog = new Views.Dialogs.NewProgram();
            if (_application.ShowDialog(dialog, "New Program", obj) == true)
            {
                _db.Programs.SaveLauncherProgram(obj);
            }

        }

        private void Add()
        {
            var dialog = new Views.Dialogs.NewProgram();
            var model = new LauncherProgram();
            if (_application.ShowDialog(dialog, "New Program", model) == true)
            {
                _db.Programs.SaveLauncherProgram(model);
            }
        }
    }
}
