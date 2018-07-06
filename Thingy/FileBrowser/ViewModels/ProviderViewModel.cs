using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Thingy.API;
using Thingy.Db;
using Thingy.Implementation;

namespace Thingy.FileBrowser.ViewModels
{
    internal class ProviderViewModel: ViewModel
    {
        private readonly IApplication _app;
        private readonly IDataBase _db;
        private readonly IModuleLoader _moduleLoader;

        private string _selected;
        private string _currentfolder;

        public ObservableCollection<string> ExternalPrograms { get; }
        public ObservableCollection<string> Places { get; }
        public ObservableCollection<string> Modules { get; }
        public ObservableCollection<string> FolderModules { get; }

        public ProviderViewModel(IApplication app, IDataBase db, IModuleLoader moduleLoader)
        {
            _app = app;
            _db = db;
            _moduleLoader = moduleLoader;
            ExternalPrograms = new ObservableCollection<string>();
            Places = new ObservableCollection<string>();
            Modules = new ObservableCollection<string>();
            FolderModules = new ObservableCollection<string>();
            InitDbParts();
        }

        public string SelectedPath
        {
            get { return _selected; }
            set
            {
                SetValue(ref _selected, value);
                GetModules(value);
            }
        }

        public string CurrentFolder
        {
            get { return _currentfolder; }
            set
            {
                SetValue(ref _currentfolder, value);
                GetFolderModules(value);
            }
        }

        private void InitDbParts()
        {
            var q1 = _db.FavoriteFolders.GetAll().Select(x => x.Name);
            var q2 = _db.Programs.GetAll().Select(x => x.Name);
            Places.UpdateWith(q1);
            ExternalPrograms.UpdateWith(q2);
        }

        private void GetModules(string selectedPath)
        {
            var modules = _moduleLoader.GetModulesForFiles(new string[] { selectedPath });
            var items = modules.Select(m => m.ModuleName);
            Modules.UpdateWith(items);
        }

        private void GetFolderModules(string value)
        {
            var modules = _moduleLoader.GetModulesWithDirectorySupport().Select(m => m.ModuleName);
            FolderModules.UpdateWith(modules);
        }

        public void StartModule(string moduleName, string parameters)
        {
            _app.HandleFiles(moduleName, new List<string> { parameters });
        }

        public void StartProgram(string programName)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedPath)) return;
                var program = _db.Programs.GetByKey(programName);
                if (program == null)
                {
                    _app.Log.Error("Program not found in db: {0}", programName);
                    return;
                }
                Process p = new Process();
                p.StartInfo.FileName = program.Path;
                p.StartInfo.Arguments = $"{program.Params} {SelectedPath}";
                p.Start();
            }
            catch (Exception ex)
            {
                _app.Log.Error("Error starting program: {0}", programName);
                _app.Log.Error(ex);
            }
        }
    }
}
