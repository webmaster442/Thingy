using AppLib.MVVM;

namespace Thingy.Views
{
    public interface ICalculatorView: IView
    {
        void SwitchToMainKeyboard();
        void FocusFormulaInput();
    }
}
