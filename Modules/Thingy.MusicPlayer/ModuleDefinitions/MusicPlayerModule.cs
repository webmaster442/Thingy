using AppLib.WPF;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
using Thingy.MusicPlayerCore;
using Thingy.MusicPlayerCore.Formats;

namespace Thingy.Modules
{
    public class MusicPlayerModule : ModuleBase
    {
        private static IAudioEngine _audioEngine;
        private IExtensionProvider _provider;

        public override string ModuleName
        {
            get { return "Music Player"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-boombox-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Media; }
        }

        public override UserControl RunModule()
        {
            var view = new MusicPlayer.Views.MusicPlayerView();
            view.DataContext = new MusicPlayer.ViewModels.MusicPlayerViewModel(view, App, _audioEngine);
            return view;
        }

        public override bool IsSingleInstance
        {
            get { return true; }
        }

        public override bool CanLoad
        {
            get
            {
                if (_audioEngine == null)
                    _audioEngine = new AudioEngine();

                return _audioEngine.OutputDevices.Count > 0;
            }
        }

        public override bool CanHadleFile(string pathOrExtension)
        {
            if (_provider == null)
                _provider = new ExtensionProvider();

            var extension = System.IO.Path.GetExtension(pathOrExtension);

            if (pathOrExtension.StartsWith("http://") || pathOrExtension.StartsWith("https://"))
                return true;

            return _provider.AllSupportedFormats.Contains(extension);
        }
    }
}
