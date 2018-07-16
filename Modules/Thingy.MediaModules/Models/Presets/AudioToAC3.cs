using Thingy.MediaModules.Controls.PresetControls;

namespace Thingy.MediaModules.Models.Presets
{
    public class AudioToAC3 : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Audio To AC3"; }
        }

        public override string Description
        {
            get { return "Convert a file to Dolby Digital compatible AC3"; }
        }

        public override string SudgestedExtension
        {
            get { return "ac3"; }
        }

        public override string AudioCommandLine
        {
            get
            {
                var bitrate = this["bitrate.Value"];
                return $"-codec:a ac3 -b:a {bitrate}k";
            }
        }

        public AudioToAC3() : base()
        {
            SliderControl slider = new SliderControl("bitrate")
            {
                Description = "Audio Bitrate in kBps",
                Minimum = 160,
                IntegersOnly = true,
                Maximum = 640,
                Value = 448
            };
            Controls.Add(slider);
        }
    }
}
