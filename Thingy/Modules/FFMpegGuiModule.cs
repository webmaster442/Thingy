using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.Infrastructure;

namespace Thingy.Modules
{
    public class FFMpegGuiModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "FFMpegGui"; }
        }

        public override ImageSource Icon => throw new NotImplementedException();

        public override string Category => throw new NotImplementedException();

        public override UserControl RunModule()
        {
            throw new NotImplementedException();
        }
    }
}
