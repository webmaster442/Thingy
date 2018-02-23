namespace Thingy.FFMpegGui.Presets
{
    public class AudioToALAC : BaseAudioPreset
    {
        public override string Name
        {
            get { return "Audio to ALAC"; }
        }

        public override string Description
        {
            get { return "Convert audio to Apple losless format. To be fully compatible choose an m4a extension"; }
        }

        public override string SudgestedExtension
        {
            get { return "m4a"; }
        }

        public override string AudioCommandLine
        {
            get { return "-codec:a alac"; }
        }
    }
}
