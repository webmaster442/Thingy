using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.FFMpegGui;

namespace Thingy.ViewModels
{
    public class FFMpegGuiViewModel: ViewModel
    {
        public PresetList Presets
        {
            get;
            set;
        }

        public FFMpegGuiViewModel()
        {
            Presets = new PresetList();
        }
    }
}
