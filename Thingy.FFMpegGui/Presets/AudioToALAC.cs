namespace Thingy.FFMpegGui.Presets
{
    public class AudioToALAC : Preset
    {
        public override string Name
        {
            get { return "Audio to ALAC"; }
        }

        public override string Description
        {
            get { return "Convert audio to Apple losless format. To be fully compatible choose an m4a extension"; }
        }

        public override string CommandLine
        {
            get { return $"ffmpeg.exe -i \"{InputFile}\" -vn -acodec alac \"{OutputFile}\""; }
        }

        public override string SudgestedExtension
        {
            get { return "m4a"; }
        }
    }
}
