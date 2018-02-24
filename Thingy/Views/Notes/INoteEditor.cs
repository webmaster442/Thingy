using AppLib.MVVM;

namespace Thingy.Views.Notes
{
    public interface INoteEditor: IView
    {
        void ClearText();
        void LoadFile(string file);
        void SaveFile(string file);
        void Print(string filename);
    }
}
