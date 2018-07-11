using System.Windows.Controls;

namespace Thingy.MediaModules.Views
{
    /// <summary>
    /// Interaction logic for FFMpegGui.xaml
    /// </summary>
    public partial class FFMpegGui : UserControl
    {
        public FFMpegGui()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTab.SelectedIndex == MainTab.Items.Count - 2)
            {
                var extension = (DataContext as ViewModels.FFMpegGuiViewModel)?.SelectedPreset?.SudgestedExtension;
                if (string.IsNullOrEmpty(extension)) extension = "[E]";
                Renamer.ExtensionPattern = extension;
            }
            if (MainTab.SelectedIndex == MainTab.Items.Count -1)
            {
                (DataContext as ViewModels.FFMpegGuiViewModel)?.GenerateBachCommand.Execute(null);
            }
        }
    }
}
