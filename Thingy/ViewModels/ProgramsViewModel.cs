using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.ViewModels
{
    public class ProgramsViewModel: ViewModel
    {
        private IDataBase _db;
        private IApplication _application;
        private string _filter;

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<string> EditCommand { get; private set; }
        public DelegateCommand<string> DeleteCommand { get; private set; }
        public DelegateCommand<string> RunCommand { get; private set; }

        public ObservableCollection<LauncherProgram> Programs { get; private set; }

        public string Filter
        {
            get { return _filter; }
            set
            {
                SetValue(ref _filter, value);
                ApplyFiltering();
            }
        }

        private void ApplyFiltering()
        {
            if (string.IsNullOrEmpty(_filter))
                Programs.UpdateWith(_db.Programs.GetPrograms());
            else
            {
                var match = from frolder in _db.Programs.GetPrograms()
                            where
                            frolder.Name.Contains(_filter, StringComparison.InvariantCultureIgnoreCase)
                            select frolder;
                Programs.UpdateWith(match);
            }
        }

        public ProgramsViewModel(IApplication app, IDataBase db)
        {
            _db = db;
            _application = app;
            Programs = new ObservableCollection<LauncherProgram>();
            AddCommand = Command.ToCommand(Add);
            EditCommand = Command.ToCommand<string>(Edit);
            DeleteCommand = Command.ToCommand<string>(Delete);
            RunCommand = Command.ToCommand<string>(Run);
            Programs.AddRange(_db.Programs.GetPrograms());
        }

        private void Run(string obj)
        {
            var program = Programs.Where(p => p.Name == obj).FirstOrDefault();
            Process run = new Process();
            run.StartInfo.FileName = program.Path;
            run.StartInfo.Arguments = program.Params;
            run.StartInfo.UseShellExecute = false;
            run.Start();
        }

        private void Delete(string obj)
        {
            var q = MessageBox.Show("Delete program?", "Link delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                _db.Programs.DeleteLauncherProgram(obj);
                ApplyFiltering();
            }
        }

        private void Edit(string obj)
        {
            var dialog = new Views.Dialogs.NewProgram();
            var program = _db.Programs.GetPrograms().Where(p => p.Name == obj).FirstOrDefault();
            var oldname = string.Copy(program.Name);

            if (_application.ShowDialog(dialog, "New Program", program) == true)
            {
                _db.Programs.UpdateLauncherProgramByName(oldname, program);
                ApplyFiltering();
            }

        }

        private void Add()
        {
            var dialog = new Views.Dialogs.NewProgram();
            var model = new LauncherProgram();
            if (_application.ShowDialog(dialog, "New Program", model) == true)
            {
                _db.Programs.SaveLauncherProgram(model);
                ApplyFiltering();
            }
        }
    }
}
