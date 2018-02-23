namespace Thingy.FFMpegGui.Presets
{
    public class H264VideoNvidia : BaseVideoPreset
    {
        public override string VideoCommandLine
        {
            get { return "-hwaccel cuvid -c:v h264_nvenc -preset slow -qp 25 -c:a copy"; }
        }

        public override string Name
        {
            get { return "h.264 video Nvidia - audio copy"; }
        }

        public override string Description
        {
            get { return "Convert to h.264 video. Audio stream is copied. Video Encoding is accelerated & requires an nVidia card with DirectX 11 support"; }
        }

        public override string SudgestedExtension
        {
            get { return "mkv"; }
        }
    }
}
