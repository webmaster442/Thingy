using System.Windows;
using System.Windows.Controls;

namespace Thingy.FFMpegGui.Controls
{
    public class SliderControl : BaseControl
    {
        private Slider _slider;

        public SliderControl(string name): base(name)
        {
            _slider = new Slider();
        }

        public double Minimum
        {
            get { return _slider.Minimum; }
            set { _slider.Minimum = value; }
        }

        public double Maximum
        {
            get { return _slider.Maximum; }
            set { _slider.Maximum = value; }
        }

        public double Value
        {
            get { return _slider.Value; }
            set { _slider.Value = value; }
        }

        public override FrameworkElement Visual
        {
            get { return _slider; }
        }
    }
}
