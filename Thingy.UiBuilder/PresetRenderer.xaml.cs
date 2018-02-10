using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thingy.FFMpegGui
{
    /// <summary>
    /// Interaction logic for PresetRenderer.xaml
    /// </summary>
    public partial class PresetRenderer : UserControl
    {
        public PresetRenderer()
        {
            InitializeComponent();
        }

        private TextBlock RenderText(string s, string style = null)
        {
            TextBlock text = new TextBlock();
            text.Text = s;

            if (!string.IsNullOrEmpty(style))
            {
                if (FindResource(style) is Style foundStyle)
                    text.Style = foundStyle;
            }

            return text;
        }

        public void Render(Preset preset)
        {
            if (preset == null) return;
            ContainerPanel.Children.Clear();

            ContainerPanel.Children.Add(RenderText(preset.Name));
            ContainerPanel.Children.Add(RenderText(preset.Description));

            foreach (var control in preset.Controls)
            {
                if (!string.IsNullOrEmpty(control.Description))
                {
                    ContainerPanel.Children.Add(RenderText(control.Description));
                    ContainerPanel.Children.Add(control.Visual);
                }
            }
        }
    }
}
