using AppLib.Maths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Thingy.CalculatorCore.Constants;

namespace Thingy.CalculatorCore
{
    public interface ICalculatorEngine: INotifyPropertyChanged
    {
        Task<CalculatorResult> Calculate(string commandLine);
        IEnumerable<Tuple<string, string>> FunctionsNamesAndPrototypes { get; }
        bool PreferPrefixes { get; set; }
        bool GroupByThousands { get; set; }
        TrigonometryMode TrigonometryMode { get; set; }
        IConstantDB ConstantDB { get; }
        IEnumerable<MemoryItem> GetMemory();
        bool DeleteVariableByName(string name);
        void SetVariable(string name, object value);
    }
}
