using AppLib.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;

namespace Thingy.Mpv.ModuleDefinitions
{
    public class MpvModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "MVP Player"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/mpv-logo-128.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            return new Views.MpvView(App);
        }

        public override bool CanLoad
        {
            get
            {
                string mpv = null;
                var setpath = App.Settings.Get("MPVPath", string.Empty);
                if (string.IsNullOrEmpty(setpath))
                {
                    mpv = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\mpv.exe");
                }
                else
                {
                    mpv = setpath;
                }
                bool canLoad = System.IO.File.Exists(mpv);
                if (!canLoad)
                    App.Log.Warning("Mpv Module not shown, because file not exists: {0}", mpv);

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
    }
}
