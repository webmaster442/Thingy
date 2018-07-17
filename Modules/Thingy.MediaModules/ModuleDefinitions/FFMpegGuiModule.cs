using AppLib.WPF;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.MediaModules.ModuleDefinitions
{
    public class FFMpegGuiModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "FFMpeg GUI"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-ffmpeg-96.png"); }
        }

        public override bool CanHadleFile(string pathOrExtension)
        {
            var extension = System.IO.Path.GetExtension(pathOrExtension);
            if (Formats.IsYoutubeUrl(pathOrExtension))
            {
                return true;
            }
            else if (Formats.SupportedVideoFormats.Contains(extension))
            {
                return true;
            }
            return Formats.SupportedAudioFormats.Contains(extension);
        }

        public override bool CanLoad
        {
            get
            {
                string ffmpeg = null;
                var setpath = App.Settings.Get("FFMpegPath", string.Empty);
                if (string.IsNullOrEmpty(setpath))
                {
                    ffmpeg = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\ffmpeg.exe");
                    App.Settings.Set("FFMpegPath", ffmpeg);
                }
                else
                {
                    ffmpeg = setpath;
                }
                bool canLoad = System.IO.File.Exists(ffmpeg);
                if (!canLoad)
                    App.Log.Warning("ffmpeg Module not shown, because file not exists: {0}", ffmpeg);

                return canLoad;
            }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.FFMpegGui
            {
                DataContext = new ViewModels.FFMpegGuiViewModel(App)
            };
        }
    }
}
