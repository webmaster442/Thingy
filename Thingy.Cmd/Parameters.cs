using System.Collections.Generic;

namespace Thingy.Cmd
{
    internal class Parameters
    {
        public List<string> Switches { get; set; }
        public List<string> Files { get; set; }
        public Dictionary<string, string> SwithesWithValue { get; set; }

        public bool HasSwitch(string shortName, string LongName = null)
        {
            return Switches.Contains(shortName) || Switches.Contains(LongName);
        }

        public bool HasSwitchWithValue(string shortName, string LongName = null)
        {
            return SwithesWithValue.ContainsKey(shortName) || SwithesWithValue.ContainsKey(LongName);
        }
    }
}
