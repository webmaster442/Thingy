using System.Windows;

namespace Thingy.MediaModules.Controls.PresetControls
{
    public abstract class BaseControl
    {
        public string Name { get; }

        public string Description { get; set; }

        public BaseControl(string name)
        {
            Name = name;
        }

        public abstract FrameworkElement Visual
        {
            get;
        }
    }
}
