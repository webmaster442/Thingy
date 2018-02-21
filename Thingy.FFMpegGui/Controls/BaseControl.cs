using System.Windows;

namespace Thingy.FFMpegGui.Controls
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
