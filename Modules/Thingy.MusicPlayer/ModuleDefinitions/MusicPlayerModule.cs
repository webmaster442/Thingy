using AppLib.WPF;
using System.Collections.Generic;
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

        public override IEnumerable<string> SupportedExtensions
        {
            get
            {
                IExtensionProvider provider = new ExtensionProvider();
                return provider.AllSupportedFormats;
            }
        }
    }
}
