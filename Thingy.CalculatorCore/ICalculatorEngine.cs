using AppLib.Maths;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Thingy.CalculatorCore.Constants;

namespace Thingy.CalculatorCore
{
    public interface ICalculatorEngine: INotifyPropertyChanged
    {
        Task<CalculatorResult> Calculate(string commandLine);
        IEnumerable<string> Functions { get; }
        bool PreferPrefixes { get; set; }
        bool GroupByThousands { get; set; }
        TrigonometryMode TrigonometryMode { get; set; }
        IConstantDB ConstantDB { get; }
    }
}
