using System;

namespace Thingy.CoreModules.Models
{
    internal class XKCDResponse
    {
#pragma warning disable IDE1006 // Naming Styles
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
#pragma warning restore IDE1006 // Naming Styles
    }
}
