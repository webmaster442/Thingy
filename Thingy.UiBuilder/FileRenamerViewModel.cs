using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.FFMpegGui
{
    public class FileRenamerViewModel: ViewModel
    {
        private string _filenamePattern;
        private string _extensionPattern;

        public string FilenamePattern
        {
            get { return _filenamePattern; }
            set { SetValue(ref _filenamePattern, value); }
        }

        public string ExtensionPattern
        {
            get { return _extensionPattern; }
            set { SetValue(ref _extensionPattern, value); }
        }
    }
}
