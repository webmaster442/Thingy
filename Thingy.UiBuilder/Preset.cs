using System.Collections.Generic;
using System.Linq;
using Thingy.FFMpegGui.Controls;

namespace Thingy.FFMpegGui
{
    public abstract class Preset
    {
        public List<BaseControl> Controls { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string CommandLine { get; }

        public string InputFile
        {
            get;
            set;
        }

        public string OutputFile
        {
            get;
            set;
        }

        protected string this[string query]
        {
            get
            {
                var parts = query.Split('.');
                if (parts.Length < 2) return "";

                var control = (from ctrl in Controls
                               where ctrl.Name == parts[0]
                               select ctrl).FirstOrDefault();

                if (control== null) return "";

                var property = (from prop in control.GetType().GetProperties()
                               where prop.Name == parts[1]
                               select prop).FirstOrDefault();

                if (property == null) return "";

                return property.GetValue(control).ToString();
            }
        }

        public Preset()
        {
            Controls = new List<BaseControl>();
        }
    }
}
