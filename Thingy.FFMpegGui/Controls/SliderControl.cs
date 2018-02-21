using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Thingy.FFMpegGui.Controls
{
    public class SliderControl : BaseControl
    {
        private Slider _slider;
        private Grid _grid;

        public SliderControl(string name): base(name)
        {
            FixedStops = new List<double>();
            _slider = new Slider();

            _grid = new Grid();
            _grid.ColumnDefinitions.Add(new ColumnDefinition());
            _grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Auto)
            });
            Grid.SetColumn(_slider, 0);
            TextBlock text = new TextBlock();
            text.Margin = new Thickness(10, 0, 10, 0);
            Grid.SetColumn(text, 1);

            Binding myBinding = new Binding();
            myBinding.Source = _slider;
            myBinding.Path = new PropertyPath("Value");
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(text, TextBlock.TextProperty, myBinding);
            _grid.Children.Add(_slider);
            _grid.Children.Add(text);
        }

        public List<double> FixedStops
        {
            get;
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
            get
            {
                if (FixedStops.Count > 0)
                {
                    _slider.Ticks.Clear();
                    foreach (var item in FixedStops) _slider.Ticks.Add(item);
                    _slider.IsSnapToTickEnabled = true;
                }

                return _grid;
            }
        }
    }
}
