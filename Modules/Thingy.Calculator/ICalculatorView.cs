using AppLib.MVVM;

namespace Thingy.Calculator
{
    public interface ICalculatorView: IView
    {
        void SwitchToMainKeyboard();
        void FocusFormulaInput();
    }
}
