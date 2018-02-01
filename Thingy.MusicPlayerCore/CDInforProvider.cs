using ManagedBass.Cd;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Thingy.MusicPlayerCore
{
    public class CDInforProvider
    {
        /// <summary>
        /// Get available laded cd drives indexes
        /// </summary>
        public static IEnumerable<string> AvailableCDDrives
        {
            get
            {
                var cds = from drive in DriveInfo.GetDrives()
                          where drive.DriveType == DriveType.CDRom &&
                          drive.IsReady
                          select drive.Name;

                return cds;
            }
        }

        /// <summary>
        /// List tracks on a CD drive
        /// </summary>
        /// <param name="drive">CD drive path</param>
        /// <returns>An array of playlist entry's</returns>
        public static string[] GetCdTracks(string drive)
        {
            var list = new List<string>();

            int drivecount = BassCd.DriveCount;
            int driveindex = 0;
            for (int i = 0; i < drivecount; i++)
            {

                var info = BassCd.GetInfo(i);
                if (info.DriveLetter == drive[0])
                {
                    driveindex = i;
                    break;
                }
            }

            if (BassCd.IsReady(driveindex))
            {
                var numtracks = BassCd.GetTracks(driveindex);
                var discid = BassCd.GetID(0, CDID.CDDB); //cddb connect
                for (int i = 0; i < numtracks; i++)
                {
                    var entry = string.Format("cd://{0}/{1}", driveindex, i);
                    list.Add(entry);
                }
            }
            BassCd.Release(driveindex);
            return list.ToArray();
        }
    }
}
