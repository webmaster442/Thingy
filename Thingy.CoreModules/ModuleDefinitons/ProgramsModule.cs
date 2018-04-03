using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
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
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-linking-96.png"); }
        }

        public override UserControl RunModule()
        {
            return new CoreModules.Views.Programs
            {
                DataContext = new CoreModules.ViewModels.ProgramsViewModel(App, App.Resolve<IDataBase>())
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
