using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;

namespace Thingy.Modules
{
    public class ProgramsModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Programs"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-linking-96.png")); }
        }

        public override UserControl RunModule()
        {
            return new Views.Programs
            {
                DataContext = new ViewModels.ProgramsViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }
    }
}
