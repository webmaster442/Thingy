using AppLib.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Mpv.ModuleDefinitions
{
    public class YoutubeDlModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "YoutubeDl"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-youtube-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.YoutubeDlView(App);
        }

        public override bool CanLoad
        {
            get
            {
                var youtubedl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\youtube-dl.exe");
                return System.IO.File.Exists(youtubedl);
            }
        }

        public override OpenParameters OpenParameters
        {
            get
            {
                return new OpenParameters
                {
                    DialogButtons = DialogButtons.OkCancel
                };
            }
        }
    }
}
