using Thingy.Engineering.Domain.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    internal interface IMintermTable
    {
        LogicItem[] GetSelected();
        void SetSelected(LogicItem[] vals);
        void ClearInput();
        void SwapVarnames();
        void SetAll(bool? value);
    }
}
