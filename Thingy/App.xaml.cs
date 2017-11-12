using AppLib.Common.IOC;
using AppLib.WPF.MVVM;
using System.Windows;
using System.Windows.Controls;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        public static IContainer IoCContainer { get; private set; }

        public static IApplication Instance
        {
            get { return App.Current as IApplication; }
        }

        public void Close()
        {
            App.Current.Shutdown();
        }

        public bool? ShowDialog(UserControl control, ViewModel model = null)
        {
            ModalDialog modalDialog = new ModalDialog();
            if (model != null)
                control.DataContext = model;
            modalDialog.DailogContent = control;
            return modalDialog.ShowDialog();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            IoCContainer = new Container();
            IoCContainer.RegisterSingleton<IDataBase, DataBase>();
            IoCContainer.RegisterSingleton<IModuleLoader, ModuleLoader>();
            base.OnStartup(e);
        }
    }
}
