﻿
using Thingy.MediaModules.Controls.PresetControls;

namespace Thingy.MediaModules.Models.Presets
{
    public class AudioToMp3 : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Audio To Mp3"; }
        }

        public override string Description
        {
            get { return "Convert a file to mp3. Mp3 sample rate must be be 44.1kHz or 48kHz"; }
        }

        public override string SudgestedExtension
        {
            get { return "mp3"; }
        }

        public override string AudioCommandLine
        {
            get
            {
                var bitrate = this["bitrate.Value"];
                return $"-codec:a libmp3lame -b:a {bitrate}k";
            }
        }

        public AudioToMp3() : base()
        {
            SliderControl slider = new SliderControl("bitrate")
            {
                Description = "Audio Bitrate in kBps",
                Minimum = 64,
                Maximum = 320,
                Value = 320
            };
            slider.FixedStops.AddRange(new double[] { 64, 96, 128, 160, 192, 224, 256, 320 });
            Controls.Add(slider);
        }
    }
}
