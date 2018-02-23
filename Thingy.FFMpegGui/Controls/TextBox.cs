using System.Windows;

namespace Thingy.FFMpegGui.Controls
{
    public class TextBox : BaseControl
    {
        private System.Windows.Controls.TextBox _textbox;

        public TextBox(string name): base(name)
        {
            _textbox = new System.Windows.Controls.TextBox();
        }

        public string Text
        {
            get { return _textbox.Text; }
            set { _textbox.Text = value; }
        }

        public override FrameworkElement Visual
        {
            get { return _textbox; }
        }
    }
}
