using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Db;
using Thingy.Infrastructure;

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
            return new Views.Programs
            {
                DataContext = new ViewModels.ProgramsViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
