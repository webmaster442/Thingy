using MahApps.Metro.Controls.Dialogs;
using System;
using System.Globalization;
using System.Windows.Controls;
using Thingy.CalculatorCore.UnitConversion;

namespace Thingy.Views.CalculatorDialogs
{
    /// <summary>
    /// Interaction logic for UnitConversionMessageBox.xaml
    /// </summary>
    public partial class UnitConversionMessageBox : CustomDialog
    {
        private UnitConverter _unitConverter;
        private IApplication _app;

        public UnitConversionMessageBox(IApplication application)
        {
            InitializeComponent();
            _unitConverter = new UnitConverter();
            _app = application;
            Categories.ItemsSource = _unitConverter.Categories;
            Categories.SelectedIndex = 0;
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Categories.SelectedItem is string selected)
            {
                _unitConverter.GetUnitsForCategory(selected);
                InputType.SelectedIndex = 0;
                TargetType.SelectedIndex = 1;
            }
        }

        public double InputNumber { get; set; }

        public double OutputNumber { get; set; }

        private void Recalculate()
        {
            if (InputType.SelectedItem is string source)
            {
                if (TargetType.SelectedItem is string target)
                {
                    if (Categories.SelectedItem is string category)
                    {
                        try
                        {
                            OutputNumber = _unitConverter.Convert(source, target, category, InputNumber);
                        }
                        catch (Exception)
                        {
                            OutputNumber = double.NaN;
                        }
                        ResultText.Text = OutputNumber.ToString(new CultureInfo("en-US"));
                    }
                }
            }
        }

        private void InputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recalculate();
        }

        private void TargetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recalculate();
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _app.HideMessageBox(this);
        }
    }
}
