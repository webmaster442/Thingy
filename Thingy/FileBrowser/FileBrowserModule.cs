using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.Db;
using Thingy.Implementation;

namespace Thingy.FileBrowser
{
    public class FileBrowserModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "File Explorer"; }
        }

        public override ImageSource Icon
        {
            get { return null; }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new FileBrowserView
            {
                DataContext = new ViewModels.FileBrowserViewModel(App, App.Resolve<IDataBase>(), App.Resolve<IModuleLoader>())
            };
        }
    }
}
