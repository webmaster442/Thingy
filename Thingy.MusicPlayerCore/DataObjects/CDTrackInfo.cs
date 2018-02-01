namespace Thingy.MusicPlayerCore.DataObjects
{
    public class CDTrackInfo
    {
        public int Drive { get; }
        public int Track { get; }

        public CDTrackInfo(string url)
        {
            var parts = url.Replace("cd://", "").Split('/');

            if (parts.Length < 2)
            {
                Drive = 0;
                Track = 0;
            }

            int drive;
            int track;

            if (int.TryParse(parts[0], out drive))
                Drive = drive;
            else
                Drive = 0;

            if (int.TryParse(parts[1], out track))
                Track = track;
            else
                Track = 0;
        }
    }
}
