using System.Collections.Generic;
using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui
{
    public abstract class Preset
    {
        public List<BaseControl> Controls { get; }

        public Preset()
        {
            Controls = new List<BaseControl>();
        }
    }
}
