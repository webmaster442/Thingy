using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class FFMpegGuiModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "FFMpegGui"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-ffmpeg-96.png")); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.FFMpegGui
            {
                DataContext = new ViewModels.FFMpegGuiViewModel()
            };
        }
    }
}
