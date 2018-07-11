using Thingy.MediaModules.Controls.PresetControls;

namespace Thingy.MediaModules.Models.Presets
{
    public class AudioToWavPack : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Audio to WavPack"; }
        }

        public override string Description
        {
            get
            {
                return "Convert audio to WavPack loslesss format"+
                       "Compression Level doesn't affect quality, only file size. Bigger number = smaller file, longer compression time";
            }
        }

        public override string SudgestedExtension
        {
            get { return "wv"; }
        }

        public override string AudioCommandLine
        {
            get
            {
                var level = this["compression.Value"];
                return $"-codec:a wavpack -compression_level {level}";
            }
        }

        public AudioToWavPack(): base()
        {
            SliderControl slider = new SliderControl("compression");
            slider.Description = "Compression Level";
            slider.Minimum = 1;
            slider.Maximum = 3;
            slider.Value = 2;
            slider.IntegersOnly = true;
            Controls.Add(slider);
        }
    }
}
