using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    /// <summary>
    /// Audio Engine Logging
    /// Uses own log, to not to spam main log
    /// </summary>
    public class AudioEngineLog: ObservableCollection<string>
    {
        public AudioEngineLog(): base()
        {

        }

        private void InsertText(string category, string format, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} | {1} : ", DateTime.Now.ToShortTimeString(), category);
            sb.AppendFormat(format, args);
            Add(sb.ToString());
        }

        public void Info(string format, params object[] args)
        {
            InsertText("Info", format, args);
        }

        public void Error(string format, params object[] args)
        {
            InsertText("Error", format, args);
        }

        public void Warning(string format, params object[] args)
        {
            InsertText("Warning", format, args);
        }
    }
}
