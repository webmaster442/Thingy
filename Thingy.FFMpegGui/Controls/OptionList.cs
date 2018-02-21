using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FFMpegGui.Controls
{
    public class OptionList : BaseControl
    {
        private ComboBox _comboBox;

        public OptionList(string name): base(name)
        {
            _comboBox = new ComboBox();
            Values = new Dictionary<string, string>();
            _comboBox.ItemsSource = Values.Keys;
        }

        public Dictionary<string, string> Values
        {
            get;
        }

        public int SelectedIndex
        {
            get { return _comboBox.SelectedIndex; }
            set { _comboBox.SelectedIndex = value; }
        }

        public string Value
        {
            get
            {
                if (_comboBox.SelectedIndex > -1)
                {
                    return Values[_comboBox.SelectedItem as string];
                }
                else return null;
            }
        }

        public override FrameworkElement Visual
        {
            get { return _comboBox; }
        }
    }
}
