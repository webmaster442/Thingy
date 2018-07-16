using Thingy.MediaModules.Controls.PresetControls;

namespace Thingy.MediaModules.Models.Presets
{
    public class DVDMpeg2 : BasePreset
    {
        public DVDMpeg2(): base()
        {
            OptionList aspect = new OptionList("aspect");
            aspect.Description = "Video Aspect ratio";
            aspect.Values.Add("16:9 (Widescreen)", "16:9");
            aspect.Values.Add("4:3  (Letterbox)", "4:3");
            aspect.SelectedIndex = 0;
            Controls.Add(aspect);

            OptionList standard = new OptionList("standard");
            standard.Description = "DVD standard";
            standard.Values.Add("PAL", "pal");
            standard.Values.Add("NTSC", "ntsc");
            standard.SelectedIndex = 0;
            Controls.Add(standard);

            SliderControl video = new SliderControl("videobitrate")
            {
                Description = "Video Bitrate in kBps",
                IntegersOnly = true,
                Minimum = 1500,
                Maximum = 9800,
                Value = 4000
            };
            Controls.Add(video);

            SliderControl audio = new SliderControl("audiobitrate")
            {
                Description = "Audio Bitrate in kBps",
                IntegersOnly = true,
                Minimum = 160,
                Maximum = 448,
                Value = 448
            };
            Controls.Add(audio);
        }

        public override string Name
        {
            get { return "DVD compatible file"; }
        }

        public override string Description
        {
            get { return "Generates DVD compatible MPEG2 files"; }
        }

        public override string CommandLine
        {
            get
            {
                var standard = "-target pal-dvd";
                if (this["standard.SelectedIndex"] == "1")
                    standard = "-target ntsc-dvd";

                var vb = this["videobitrate.Value"];
                var ab = this["audiobitrate.Value"];

                return $"ffmpeg.exe -i \"{InputFile}\" {standard} -aspect {this["aspect.Value"]} -b:v {vb}k -c:a ac3 -b:a {ab} -ar 48000 \"{OutputFile}\"";
            }
        }

        public override string SudgestedExtension
        {
            get { return "mpeg"; }
        }
    }
}
