using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FileBrowser.Controls
{
    /// <summary>
    /// Interaction logic for BreadcumbBar.xaml
    /// </summary>
    public partial class BreadcumbBar : UserControl, IFileSystemControl
    {
        public BreadcumbBar()
        {
            InitializeComponent();
        }

        static BreadcumbBar()
        {
            SelectedPathProperty = DependencyProperty.Register("SelectedPath",
                                                                typeof(string),
                                                                typeof(BreadcumbBar),
                                                                new FrameworkPropertyMetadata("",
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                SelectedPathChanged));
        }

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public bool IsHiddenVisible { get; set; }

        // Using a DependencyProperty as the backing store for SelectedPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPathProperty;

        public event EventHandler<string> OnNavigationException;

        private static void SelectedPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BreadcumbBar sender = d as BreadcumbBar;
            sender.BreadCumbs.Children.Clear();
            sender.Render();
        }

        private void Render()
        {
            try
            {
                string[] parts = SelectedPath?.Split('/', '\\');
                if (parts == null || parts.Length < 0) return;

                for (int i = 0; i < parts.Length; i++)
                {
                    if (string.IsNullOrEmpty(parts[i])) continue;

                    Button b = new Button
                    {
                        ToolTip = BuildPathString(parts, i),
                        Content = parts[i]
                    };
                    b.Click += B_Click;
                    BreadCumbs.Children.Add(b);

                    TextBlock div = new TextBlock();
                    div.Text = @"\";
                    BreadCumbs.Children.Add(div);
                }
            }
            catch (Exception ex)
            {
                OnNavigationException?.Invoke(this, ex.Message);
            }
        }

        private object BuildPathString(string[] parts, int i)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j <= i; j++)
            {
                sb.AppendFormat("{0}\\", parts[j]);
            }
            return sb.ToString();
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var path = b.ToolTip?.ToString();
            if (!string.IsNullOrEmpty(path))
            {
                SelectedPath = path;
            }
        }
    }
}
