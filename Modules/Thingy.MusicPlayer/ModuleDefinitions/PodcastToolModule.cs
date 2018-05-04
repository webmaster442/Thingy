using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.Db;

namespace Thingy.MusicPlayer.ModuleDefinitions
{
    public class PodcastToolModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Podcasts"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-playlist-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Media; }
        }

        public override UserControl RunModule()
        {
            return new Views.Podcasttool
            {
                DataContext = new ViewModels.PodcastToolViewModel(App, App.Resolve<IDataBase>())
            };
        }
    }
}
