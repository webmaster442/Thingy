using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;

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
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-favorite-folder-96.png")); }
        }

        public override UserControl RunModule()
        {
            return new Views.VirtualFolders
            {
                DataContext = new ViewModels.VirtualFoldersViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }
    }
}
