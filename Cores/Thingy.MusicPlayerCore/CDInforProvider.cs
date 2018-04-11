using ManagedBass.Cd;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    public class CDInfoProvider
    {
        private static string CurrentDiscID;
        public static Dictionary<string, string> CdData { get; }

        static CDInfoProvider()
        {
            CdData = new Dictionary<string, string>();
        }

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
        public static Task<string[]> GetCdTracks(string drive)
        {
            return Task.Run(() =>
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
                    if (discid != CurrentDiscID)
                    {
                        var datas = BassCd.GetIDText(driveindex);
                        CurrentDiscID = discid;
                        CdData.Clear();
                        foreach (var data in datas)
                        {
                            var item = data.Split('=');
                            CdData.Add(item[0], item[1]);
                        }
                    }

                    for (int i = 0; i < numtracks; i++)
                    {
                        var entry = string.Format("cd://{0}/{1}", driveindex, i);
                        list.Add(entry);
                    }
                }
                BassCd.Release(driveindex);
                return list.ToArray();
            });
        }
    }
}
