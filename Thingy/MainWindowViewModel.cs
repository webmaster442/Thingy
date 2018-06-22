using AppLib.MVVM;
using Dragablz;
using System;
using System.Threading.Tasks;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.Implementation;

namespace Thingy
{
    public class MainWindowViewModel : ViewModel<IMainWindow>
    {
        public DelegateCommand SettingCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand<string> OpenFlyoutCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand ModuleImportCommand { get; private set; }
        public DelegateCommand ModuleAppendCommand { get; private set; }
        public DelegateCommand ModuleExportCommand { get; private set; }

        private readonly IApplication _app;

        public MainWindowViewModel(IMainWindow view, IApplication app) : base(view)
        {
            _app = app;
            ExitCommand = Command.CreateCommand(Exit);
            AboutCommand = Command.CreateCommand(OpenAbout);
            LogCommand = Command.CreateCommand(OpenLog, CanOpenLog);
            OpenFlyoutCommand = Command.CreateCommand<string>(OpenFlyout);
            SettingCommand = Command.CreateCommand(OpenSetting, CanOpenSetting);
            UpdateCommand = Command.CreateCommand(Update);
            ModuleImportCommand = Command.CreateCommand(ModuleImport, CanImportExport);
            ModuleExportCommand = Command.CreateCommand(ModuleExport, CanImportExport);
            ModuleAppendCommand = Command.CreateCommand(ModuleAppend, CanImportExport);
        }

        private void OpenFlyout(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                View.OpenOrHideFlyout(obj);
            }
        }

        private void Update()
        {
            InternalCode.WinSparkle.win_sparkle_check_update_with_ui();
        }

        private bool CanOpenSetting()
        {
            int index = _app.TabManager.GetTabIndexByTitle("Settings");
            return index == -1;
        }

        private void OpenSetting()
        {
            _app.TabManager.CreateNewTabContent("Settings", new InternalModules.Settings(_app));
        }

        private bool CanOpenLog()
        {
            int index = _app.TabManager.GetTabIndexByTitle("Log");
            return index == -1;
        }

        private void OpenLog()
        {
            _app.TabManager.CreateNewTabContent("Log", new InternalModules.LogViewer(_app));
        }

        private bool CanImportExport()
        {
            return View?.CurrentTabContent?.DataContext is ICanImportExportXMLData;
        }

        private async void ModuleExport()
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var module = View?.CurrentTabContent?.DataContext as ICanImportExportXMLData;
                    if (module != null)
                    {
                        using (var file = System.IO.File.OpenWrite(sfd.FileName))
                        {
                            View.SetBusyOverlayVisibility(true);
                            await module.Export(file);
                            View.SetBusyOverlayVisibility(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    View.SetBusyOverlayVisibility(false);
                    await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
                }
            }
        }

        private async Task DoImport(bool append)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var module = View?.CurrentTabContent?.DataContext as ICanImportExportXMLData;
                    if (module != null)
                    {
                        using (var file = System.IO.File.OpenRead(ofd.FileName))
                        {
                            View.SetBusyOverlayVisibility(true);
                            await module.Import(file, append);
                            View.SetBusyOverlayVisibility(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    View.SetBusyOverlayVisibility(false);
                    await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
                }
            }
        }

        private async void ModuleImport()
        {
            await DoImport(false);
        }

        private async void ModuleAppend()
        {
            await DoImport(true);
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        public Func<HeaderedItemViewModel> ItemFactory
        {
            get
            {
                return () =>
                {
                    var start = new Controls.StartPage
                    {
                        DataContext = new Controls.StartPageViewModel(_app, _app.Resolve<IModuleLoader>())
                    };

                    return new HeaderedItemViewModel
                    {
                        IsSelected = true,
                        Header = "New Tool...",
                        Content = start
                    };
                };
            }
        }

        private void OpenAbout()
        {
            _app.TabManager.CreateNewTabContent("About", new InternalModules.About());
        }
    }
}
