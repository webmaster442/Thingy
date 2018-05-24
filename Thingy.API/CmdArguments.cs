using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.API
{
    public class CmdArguments
    {
        /// <summary>
        /// Standalone switches passed
        /// </summary>
        IList<string> StandaloneSwitches { get; }

        /// <summary>
        /// Files passed
        /// </summary>
        IList<string> Files { get; }

        /// <summary>
        /// Switches with values
        /// </summary>
        IDictionary<string, string> SwitchesWithValue { get; }

        public CmdArguments(IList<string> standalone, IList<string> files, IDictionary<string, string> swicthes)
        {
            StandaloneSwitches = standalone;
            Files = files;
            SwitchesWithValue = swicthes;
        }
    }
}
