using System.Windows;
using System.Windows.Controls;

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

        private Style GetStyle(string name)
        {
            return FindResource(name) as Style;
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
                    GroupBox group = new GroupBox
                    {
                        Header = control.Description,
                        Content = control.Visual,
                        Style = GetStyle("Child")
                    };
                    ContainerPanel.Children.Add(group);

                }
                else
                {
                    control.Visual.Style = GetStyle("Child");
                    ContainerPanel.Children.Add(control.Visual);
                }
            }
        }
    }
}
