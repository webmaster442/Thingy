using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui.Presets
{
    public class AudioToFLAC : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Audio to FLAC"; }
        }

        public override string Description
        {
            get
            {
                return "Convert audio to Free losless Audio codec.\r\n" +
                       "Compression Level doesn't affect quality, only file size. Bigger number = smaller file, longer compression time";
            }
        }

        public override string SudgestedExtension
        {
            get { return "flac"; }
        }

        public override string AudioCommandLine
        {
            get
            {
                var level = this["compression.Value"];
                return $"-codec:a flac -compression_level {level}";
            }
        }

        public AudioToFLAC(): base()
        {
            SliderControl slider = new SliderControl("compression");
            slider.Description = "Compression Level";
            slider.Minimum = 1;
            slider.Maximum = 12;
            slider.Value = 8;
            slider.IntegersOnly = true;
            Controls.Add(slider);
        }
    }
}
