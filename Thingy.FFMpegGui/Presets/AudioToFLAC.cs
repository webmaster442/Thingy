namespace Thingy.FFMpegGui.Presets
{
    public class AudioToFLAC : Preset
    {
        public override string Name
        {
            get { return "Audio to FLAC"; }
        }

        public override string Description
        {
            get { return "Convert audio to Free losless Audio codec."; }
        }

        public override string CommandLine
        {
            get { return $"ffmpeg.exe -i \"{InputFile}\" -vn -codec:a flac \"{OutputFile}\""; }
        }

        public override string SudgestedExtension
        {
            get { return "flac"; }
        }
    }
}
