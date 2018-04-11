using System;

namespace Thingy.CoreModules.Models
{
    public class XKCDResponse
    {
        public int month { get; set; }
        public int num { get; set; }
        public string link { get; set; }
        public int year { get; set; }
        public string news { get; set; }
        public string safe_title { get; set; }
        public string transcript { get; set; }
        public string alt { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public int day { get; set; }

        public DateTime PublicationDate
        {
            get { return new DateTime(year, month, day).Date; }
        }
    }
}
