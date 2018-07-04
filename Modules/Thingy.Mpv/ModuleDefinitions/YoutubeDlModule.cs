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
            get { return ModuleCategories.Media; }
        }

        public override UserControl RunModule()
        {
            return new Views.YoutubeDlView(App);
        }

        public override bool CanLoad
        {
            get
            {
                string youtubedl = null;
                var setpath = App.Settings.Get("YoutubeDlPath", string.Empty);
                if (string.IsNullOrEmpty(setpath))
                {
                    youtubedl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\youtube-dl.exe");
                }
                else
                {
                    youtubedl = setpath;
                }

                bool canLoad = System.IO.File.Exists(youtubedl);
                if (!canLoad)
                    App.Log.Warning("youtube-dl Module not shown, because file not exists: {0}", youtubedl);
                return canLoad;
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

        public override bool CanHadleFile(string pathOrExtension)
        {
            return MpvFormats.IsYoutubeUrl(pathOrExtension);
        }
    }
}
