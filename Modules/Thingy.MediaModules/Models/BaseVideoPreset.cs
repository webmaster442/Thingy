using Thingy.MediaModules.Controls.PresetControls;

namespace Thingy.MediaModules.Models
{
    public abstract class BaseVideoPreset : BasePreset
    {
        public abstract string VideoCommandLine { get; }

        public override string CommandLine
        {
            get
            {
                var resolution = this["resolutions.Value"];
                var framerate = this["framerate.Value"];

                var advanced = $"{resolution} {framerate}".Trim();

                var command = "ffmpeg.exe";

                if (!string.IsNullOrEmpty(ParamsBeforeInputFile))
                    command = $"ffmpeg.exe {ParamsBeforeInputFile} -i";

                if (!string.IsNullOrEmpty(advanced))
                    return $"{command} \"{InputFile}\" {advanced} {VideoCommandLine} \"{OutputFile}\"";
                else
                    return $"{command} \"{InputFile}\" {VideoCommandLine} \"{OutputFile}\"";
            }
        }

        public virtual string ParamsBeforeInputFile
        {
            get;
        }

        public BaseVideoPreset() : base()
        {

            OptionList resolutons = new OptionList("resolutions");
            resolutons.Description = "Video Resolution";
            resolutons.Values.Add("Same as input", "");
            resolutons.Values.Add("320 x 240 (4:3)", "-vf scale=320:240");
            resolutons.Values.Add("640 x 480 (4:3)", "-vf scale=640:480");
            resolutons.Values.Add("720 x 480 (4:3 NTSC DVD)", "-vf scale=720:480 -aspect 4:3");
            resolutons.Values.Add("720 x 480 (16:9 NTSC DVD)", "-vf scale=720:480 -aspect 16:9");
            resolutons.Values.Add("720 x 576 (4:3 PAL DVD)", "-vf scale=720:576 -aspect 4:3");
            resolutons.Values.Add("720 x 576 (16:9 PAL DVD)", "-vf scale=720:576 -aspect 16:9");
            resolutons.Values.Add("800 x 600 (4:3)", "-vf scale=800:600");
            resolutons.Values.Add("854 x 480 (16:9)", "-vf scale=854:480");
            resolutons.Values.Add("1280 x 720 (16:9)", "-vf scale=1280:720");
            resolutons.Values.Add("1920 x 1080 (16:9)", "-vf scale=1920:1080");
            resolutons.Values.Add("3840 x 2160 (16:9)", "-vf scale=3840:2160");
            resolutons.SelectedIndex = 0;

            OptionList framerate = new OptionList("framerate");
            framerate.Description = "Video Framerate";
            framerate.Values.Add("Same as input", "");
            framerate.Values.Add("24 fps", "-framerate 24");
            framerate.Values.Add("25 fps", "-framerate 25");
            framerate.Values.Add("29.97 fps", "-framerate 29.97");
            framerate.Values.Add("30 fps", "-framerate 30");
            framerate.Values.Add("50 fps", "-framerate 50");
            framerate.Values.Add("59.94 fps", "-framerate 59.94");
            framerate.Values.Add("60 fps", "-framerate 60");
            framerate.SelectedIndex = 0;

            Controls.Add(resolutons);
            Controls.Add(framerate);
        }
    }
}
