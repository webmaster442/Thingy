using System.Collections.Generic;
using Thingy.Db.Entity.MediaLibary;

namespace Thingy.Db.initialData
{
    internal static class RadioStations
    {
        public static IEnumerable<RadioStation> Collection
        {
            get
            {
                yield return new RadioStation("Q - Dance radio", "https://19993.live.streamtheworld.com/Q_DANCEAAC.aac");
                yield return new RadioStation("Hardcore radio", "http://shoutcast1.hardcoreradio.nl");
                yield return new RadioStation("Bartók Rádió", "http://mr-stream.mediaconnect.hu/4742/mr3hq.mp3");
                yield return new RadioStation("Dankó Rádió", "http://mr-stream.mediaconnect.hu/4750/mr6p.mp3");
                yield return new RadioStation("Kossuth Rádió", "http://mr-stream.mediaconnect.hu/4736/mr1.mp3");
                yield return new RadioStation("Petőfi Rádió", "http://mr-stream.mediaconnect.hu/4738/mr2.mp3");
                yield return new RadioStation("Slam!Hardstyle", "http://streaming.slam.nl/web11_aac");
                yield return new RadioStation("Slam!Live", "http://stream.slam.nl/slamaac");
                yield return new RadioStation("Hardstyle FM", "http://93.190.142.179:8280/stream");
                yield return new RadioStation("Real Hardstyle Radio(RHR)", "http://stream.rhr.fm/");
            }
        }
    }
}
