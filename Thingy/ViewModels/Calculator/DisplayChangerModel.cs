using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.ViewModels.Calculator
{
    public class DisplayChangerModel : BindableBase
    {
        public DelegateCommand<object> ConvertFileSizeCommand { get; private set; }
        public DelegateCommand<object> ConvertPercentageCommand { get; private set; }
        public DelegateCommand<object> ConvertTextCommand { get; private set; }
        public DelegateCommand<object> ConvertFractionsCommand { get; private set; }
        public DelegateCommand<object> ConvertPrefixesCommand { get; private set; }
        public DelegateCommand<object> ConvertNumberSystemCommand { get; private set; }

        public DisplayChangerModel()
        {
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
                    return false;
            }
        }

        private double ConvertBeforeProcess(object obj)
        {
            return Convert.ToDouble(obj);
        }

        private void ConvertFileSize(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConvertPercentage(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConvertText(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConvertFractions(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConvertPrefixes(object obj)
        {
            throw new NotImplementedException();
        }

        private void ConvertNumberSystem(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
