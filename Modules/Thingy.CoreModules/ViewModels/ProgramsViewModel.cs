using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.CoreModules.Models;
using Thingy.Db;
using Thingy.Db.Entity;

namespace Thingy.CoreModules.ViewModels
{
    public class ProgramsViewModel: ViewModel, ICanImportExportXMLData
    {
        private IDataBase _db;
        private IApplication _application;
        private string _filter;
        private string _startmenufilter;

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<string> EditCommand { get; private set; }
        public DelegateCommand<string> DeleteCommand { get; private set; }
        public DelegateCommand<string> RunCommand { get; private set; }
        public DelegateCommand<string> RunSystemCommand { get; private set; }

        public ObservableCollection<LauncherProgram> Programs { get; private set; }
        public ObservableCollection<SystemProgram> StartMenu { get; private set; }

        public string Filter
        {
            get { return _filter; }
            set
            {
                SetValue(ref _filter, value);
                ApplyFiltering();
            }
        }

        public string StartMenuFilter
        {
            get { return _startmenufilter; }
            set
            {
                SetValue(ref _startmenufilter, value);
                ApplyStartMenuFilter();
            }
        }

        private void ApplyStartMenuFilter()
        {
            if (string.IsNullOrEmpty(_startmenufilter))
                StartMenu.UpdateWith(ProgramProviders.GetStartMenu());
            else
            {
                var match = from program in ProgramProviders.GetStartMenu()
                            where
                            program.Name.Contains(_startmenufilter, StringComparison.InvariantCultureIgnoreCase)
                            select program;
                StartMenu.UpdateWith(match);
            }

        }

        private void ApplyFiltering()
        {
            if (string.IsNullOrEmpty(_filter))
                Programs.UpdateWith(_db.Programs.GetAll());
            else
            {
                var match = from frolder in _db.Programs.GetAll()
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
            AddCommand = Command.CreateCommand(Add);
            EditCommand = Command.CreateCommand<string>(Edit);
            DeleteCommand = Command.CreateCommand<string>(Delete);
            RunCommand = Command.CreateCommand<string>(Run);
            RunSystemCommand = Command.CreateCommand<string>(RunSystem);
            Programs.AddRange(_db.Programs.GetAll());
            StartMenu = new ObservableCollection<SystemProgram>(ProgramProviders.GetStartMenu());
        }

        private void RunSystem(string obj)
        {
            Process.Start(obj);
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

        private async void Delete(string obj)
        {
            var q = await _application.ShowMessageBox("Link delete", "Delete program?", DialogButtons.YesNo);
            if (q)
            {
                _db.Programs.Delete(obj);
                ApplyFiltering();
            }
        }

        private async void Edit(string obj)
        {
            var dialog = new CoreModules.Dialogs.NewProgram();
            var program = _db.Programs.GetAll().Where(p => p.Name == obj).FirstOrDefault();
            var oldname = string.Copy(program.Name);
            var result = await _application.ShowDialog("New Program", dialog, DialogButtons.OkCancel, true, program);

            if (result)
            {
                _db.Programs.UpdateLauncherProgramByName(oldname, program);
                ApplyFiltering();
            }

        }

        private async void Add()
        {
            var dialog = new CoreModules.Dialogs.NewProgram();
            var model = new LauncherProgram();
            var result = await _application.ShowDialog("New Program", dialog, DialogButtons.OkCancel, true, model);
            if (result)
            {
                _db.Programs.Save(model);
                ApplyFiltering();
            }
        }

        public Task Import(Stream xmlData, bool append)
        {
            return Task.Run(() =>
            {
                var import = EntitySerializer.Deserialize<LauncherProgram[]>(xmlData);
                if (append)
                    _db.Programs.Save(import);
                else
                {
                    _db.Programs.DeleteAll();
                    _db.Programs.Save(import);
                }
                _application.CurrentDispatcher.Invoke(() => Programs.UpdateWith(_db.Programs.GetAll()));
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
