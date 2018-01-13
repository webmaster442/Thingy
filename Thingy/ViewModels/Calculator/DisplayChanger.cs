using AppLib.Maths;
using AppLib.MVVM;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.CalculatorCore;

namespace Thingy.ViewModels.Calculator
{
    public class DisplayChangerModel : BindableBase
    {
        private IApplication _app;

        public DelegateCommand<object> ConvertFileSizeCommand { get; private set; }
        public DelegateCommand<object> ConvertPercentageCommand { get; private set; }
        public DelegateCommand<object> ConvertTextCommand { get; private set; }
        public DelegateCommand<object> ConvertFractionsCommand { get; private set; }
        public DelegateCommand<object> ConvertPrefixesCommand { get; private set; }
        public DelegateCommand<object> ConvertNumberSystemCommand { get; private set; }

        public DisplayChangerModel(IApplication app)
        {
            _app = app;
            ConvertFileSizeCommand = Command.ToCommand<object>(ConvertFileSize, CanExecute);
            ConvertPercentageCommand = Command.ToCommand<object>(ConvertPercentage, CanExecute);
            ConvertTextCommand = Command.ToCommand<object>(ConvertText, CanExecute);
            ConvertFractionsCommand = Command.ToCommand<object>(ConvertFractions, CanExecute);
            ConvertPrefixesCommand = Command.ToCommand<object>(ConvertPrefixes, CanExecute);
            ConvertNumberSystemCommand = Command.ToCommand<object>(ConvertNumberSystem, CanExecute);
        }

        private bool CanExecute(object obj)
        {
            Type t = obj.GetType();
            switch (t.Name)
            {
                case "Double":
                case "Single":
                case "Int32":
                case "Int16":
                case "Byte":
                case "SByte":
                case "UInt32":
                case "UInt64":
                    return true;
                default:
                    if (obj is Fraction) return true;
                    else return false;
            }
        }

        private double ConvertBeforeProcess(object obj)
        {
            return Convert.ToDouble(obj);
        }

        private async void ConvertFileSize(object obj)
        {
            double value = ConvertBeforeProcess(obj);
            string prefix = "Byte";

            if (value > 1180591620717411303424.0)
            {
                value /= 1180591620717411303424.0;
                prefix = "ZiB";
            }
            if (value > 1152921504606846976.0)
            {
                value /= 1152921504606846976.0;
                prefix = "EiB";
            }
            if (value > 1125899906842624.0)
            {
                value /= 1125899906842624.0;
                prefix = "PiB";
            }
            else if (value > 1099511627776.0)
            {
                value /= 1099511627776.0;
                prefix = "TiB";
            }
            else if (value > 1073741824.0)
            {
                value /= 1073741824.0;
                prefix = "GiB";
            }
            else if (value > 1048576.0)
            {
                value /= 1048576.0;
                prefix = "MiB";
            }
            else if (value > 1024)
            {
                value /= 1024;
                prefix = "kiB";
            }
            var content = string.Format("{0} {1}", value, prefix);
            await _app.ShowMessageBox("Result as File size", content, MessageDialogStyle.Affirmative);

        }

        private async void ConvertPercentage(object obj)
        {
            double x = ConvertBeforeProcess(obj) * 100;
            var message = string.Format("{0}%", x);
            await _app.ShowMessageBox("Result as percent", message, MessageDialogStyle.Affirmative);
        }

        private async void ConvertFractions(object obj)
        {
            var dbl = ConvertBeforeProcess(obj);
            Fraction f = dbl;
            await _app.ShowMessageBox("Result as fraction", f.ToString(), MessageDialogStyle.Affirmative);
        }

        private async void ConvertPrefixes(object obj)
        {
            var dbl = ConvertBeforeProcess(obj);
            string prefixed = PrefixSource.DivideToPrefix(dbl);
            await _app.ShowMessageBox("Result with prefixes", prefixed, MessageDialogStyle.Affirmative);
        }

        private async void ConvertText(object obj)
        {
            var content = new Views.CalculatorDialogs.NumberToTexMessageBox(_app);
            content.SetDisplay(obj);
            await _app.ShowMessageBox(content);
        }

        private async void ConvertNumberSystem(object obj)
        {
            var content = new Views.CalculatorDialogs.NumberSystemDisplayMessageBox(_app);
            content.SetDisplay(obj);
            await _app.ShowMessageBox(content);
        }
    }
}
