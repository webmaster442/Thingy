using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FFMpegGui
{
    /// <summary>
    /// Interaction logic for FileRenamer.xaml
    /// </summary>
    public partial class FileRenamer : UserControl
    {
        public FileRenamer()
        {
            InitializeComponent();
        }

        public IEnumerable<string> InputFiles
        {
            get { return (IEnumerable<string>)GetValue(InputFilesProperty); }
            set { SetValue(InputFilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputFiles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputFilesProperty =
            DependencyProperty.Register("InputFiles", typeof(IEnumerable<string>), typeof(FileRenamer), new PropertyMetadata(null, InputFilesChanged));

        private static void InputFilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FileRenamer;
            var model = ctrl.DataContext as FileRenamerViewModel;
            if (model != null)
            {
                model.InputFiles = e.NewValue as IEnumerable<string>;
            }
        }

        public string OutputFolder
        {
            get { return (string)GetValue(OutputFolderProperty); }
            set { SetValue(OutputFolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputFolder This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputFolderProperty =
            DependencyProperty.Register("OutputFolder", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, OutputFolderChanged));

        private static void OutputFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FileRenamer;
            var model = ctrl.DataContext as FileRenamerViewModel;
            if (model != null)
            {
                model.OutputFolder = e.NewValue as string;
            }
        }
    }
}
