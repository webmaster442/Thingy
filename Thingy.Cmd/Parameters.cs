using System.Collections.Generic;

namespace Thingy.Cmd
{
    public class Parameters
    {
        public Parameters()
        {
            Switches = new List<string>();
            Files = new List<string>();
            SwithesWithValue = new Dictionary<string, string>();
        }

        public List<string> Switches { get; }

        public List<string> Files { get; }

        public Dictionary<string, string> SwithesWithValue { get; }

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
