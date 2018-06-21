using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FileBrowser.Controls
{
    internal class BreadcumbBar: StackPanel, IFileSystemControl
    {
        static BreadcumbBar()
        {
            SelectedPathProperty = DependencyProperty.Register("SelectedPath", 
                                                                typeof(string), 
                                                                typeof(BreadcumbBar), 
                                                                new FrameworkPropertyMetadata("", 
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                                                                SelectedPathChanged));
        }

        public BreadcumbBar()
        {
            Orientation = Orientation.Horizontal;
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
            sender.Children.Clear();
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
                    Button b = new Button();
                    b.ToolTip = BuildPathString(parts, i);
                    b.Content = parts[i];
                    b.Click += B_Click;
                    Children.Add(b);

                    TextBlock div = new TextBlock();
                    div.Text = @"\";
                    Children.Add(div);
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
            for (int j=0; j<i; j++)
            {
                sb.AppendFormat("{0}\\", parts[i]);
            }
            return sb.ToString();
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var path = b.Tag?.ToString();
            if (!string.IsNullOrEmpty(path))
            {
                SelectedPath = path;
            }
        }
    }
}
