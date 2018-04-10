using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class MediaLibaryModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Media Libary"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-loudspeaker-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.MediaLibary.MediaLibary
            {
                DataContext = new ViewModels.MediaLibary.MediaLibaryViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }
    }
}
