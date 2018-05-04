using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.Db;

namespace Thingy.MusicPlayer.ModuleDefinitions
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
            get { return ModuleCategories.Media; }
        }

        public override UserControl RunModule()
        {
            return new Views.MediaLibary
            {
                DataContext = new ViewModels.MediaLibaryViewModel(App, App.Resolve<IDataBase>())
            };
        }
    }
}
