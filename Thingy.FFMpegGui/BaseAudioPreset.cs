using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui
{
    public abstract class BaseAudioPreset : BasePreset
    {
        public override string CommandLine
        {
            get
            {
                var resample = this["resamplerate.Value"];
               
                if (resample == "sameasinput")
                    return $"ffmpeg.exe -i \"{InputFile}\" -vn {AudioCommandLine} \"{OutputFile}\"";
                else
                    return $"ffmpeg.exe -i \"{InputFile}\" -vn -ar {resample} {AudioCommandLine} \"{OutputFile}\"";
            }
        }

        public abstract string AudioCommandLine { get; }

        public BaseAudioPreset(): base()
        {
            OptionList resample = new OptionList("resamplerate");
            resample.Description = "Output sample rate";
            resample.Values.Add("Same as input", "sameasinput");
            resample.Values.Add("22.5 kHz", "22500");
            resample.Values.Add("44.1 kHz", "44100");
            resample.Values.Add("48 kHz", "48000");
            resample.Values.Add("96 kHz", "96000");
            resample.Values.Add("192 kHz", "192000");
            resample.SelectedIndex = 0;
            Controls.Add(resample);
        }
    }
}
