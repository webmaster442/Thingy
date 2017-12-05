using System.Windows.Controls;
using System.Windows.Media;
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
            get { return null; }
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
