using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.Infrastructure;

namespace Thingy.ViewModels
{
    public class ProgramsViewModel: ViewModel, ICanImportExportXMLData
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

        private async void Edit(string obj)
        {
            var dialog = new Views.Dialogs.NewProgram();
            var program = _db.Programs.GetPrograms().Where(p => p.Name == obj).FirstOrDefault();
            var oldname = string.Copy(program.Name);
            var result = await _application.ShowDialog(dialog, "New Program", program);

            if (result)
            {
                _db.Programs.UpdateLauncherProgramByName(oldname, program);
                ApplyFiltering();
            }

        }

        private async void Add()
        {
            var dialog = new Views.Dialogs.NewProgram();
            var model = new LauncherProgram();
            var result = await _application.ShowDialog(dialog, "New Program", model);
            if (result)
            {
                _db.Programs.SaveLauncherProgram(model);
                ApplyFiltering();
            }
        }

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<LauncherProgram[]>(xmlData);
                if (append)
                    _db.Programs.SaveLauncherPrograms(import);
                else
                {
                    _db.Programs.DeleteAll();
                    _db.Programs.SaveLauncherPrograms(import);
                }
                App.Current.Dispatcher.Invoke(() => Programs.UpdateWith(_db.Programs.GetPrograms()));
            });
        }

        public Task Export(Stream xmlData)
        {
            return Task.Run(() =>
            {
                EntitySerializer.Serialize(xmlData, Programs.ToArray());
            });
        }
    }
}
