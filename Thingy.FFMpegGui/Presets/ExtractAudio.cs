using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui.Presets
{
    public class ExtractAudio : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Extract Audio"; }
        }

        public override string Description
        {
            get { return "Extract Audio from video stream"; }
        }

        public override string SudgestedExtension
        {
            get
            {
                var format = this["formatoptions.SelectedIndex"];
                if (format == "0")
                    return "???";
                else
                    return "wav";
            }
        }

        public override string AudioCommandLine
        {
            get
            {
                var format = this["formatoptions.SelectedIndex"];
                if (format == "0")
                    return "-acodec copy";
                else
                    return "-f wav";
            }
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
