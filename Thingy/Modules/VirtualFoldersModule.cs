using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class VirtualFoldersModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Virtual Folders"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-favorite-folder-96.png"); }
        }

        public override UserControl RunModule()
        {
            return new Views.VirtualFolders
            {
                DataContext = new ViewModels.VirtualFoldersViewModel(App.Instance,
                                                                     App.IoCContainer.ResolveSingleton<IDataBase>(),
                                                                     App.Log)
            };
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
