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
        public static readonly DependencyProperty PresetProperty =
            DependencyProperty.Register("Preset", typeof(Preset), typeof(PresetRenderer), new PropertyMetadata(null, PresetChanged));

        public Preset Preset
        {
            get { return (Preset)GetValue(PresetProperty); }
            set { SetValue(PresetProperty, value); }
        }

        private static void PresetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PresetRenderer caller = d as PresetRenderer;
            var presetToRender = e.NewValue as Preset;
            caller.Render(presetToRender);
        }

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

        private void Render(Preset preset)
        {
            ContainerPanel.Children.Clear();
            if (preset == null) return;

            ContainerPanel.Children.Add(RenderText(preset.Name, "Name"));
            ContainerPanel.Children.Add(RenderText(preset.Description, "Description"));

            foreach (var control in preset.Controls)
            {
                if (!string.IsNullOrEmpty(control.Description))
                {
                    GroupBox group = new GroupBox();
                    group.Header = control.Description;
                    group.Content = control.Visual;
                    ContainerPanel.Children.Add(group);

                }
                else
                {
                    ContainerPanel.Children.Add(control.Visual);
                }
            }
        }
    }
}
