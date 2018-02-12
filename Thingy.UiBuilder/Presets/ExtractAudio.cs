using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui.Presets
{
    public class ExtractAudio : Preset
    {
        public override string Name
        {
            get { return "Extract Audio"; }
        }

        public override string Description
        {
            get { return "Extract Audio from video stream"; }
        }

        public override string CommandLine
        {
            get { return ""; }
        }

        public ExtractAudio(): base()
        {
            OptionList formatoptions = new OptionList("formatoptions");
            formatoptions.Description = "Output format";
            formatoptions.Values.Add("Same as input", "sameasinput");
            formatoptions.Values.Add("WAV - 16 bit 44.1khz", "wav");
            formatoptions.SelectedIndex = 0;

            Controls.Add(formatoptions);
        }
    }
}
