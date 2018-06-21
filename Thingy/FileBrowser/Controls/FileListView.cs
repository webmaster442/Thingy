using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FileBrowser.Controls
{
    internal class FileListView: ListView, IFileDisplayControl
    {

        static FileListView()
        {
            SelectedPathProperty = DependencyProperty.Register("SelectedPath",
                                                                typeof(string),
                                                                typeof(FileListView),
                                                                new FrameworkPropertyMetadata("",
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                Render));

            IsHiddenVisibleProperty = DependencyProperty.Register("IsHiddenVisible",
                                                                  typeof(bool),
                                                                  typeof(FileListView),
                                                                  new FrameworkPropertyMetadata(false,
                                                                  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                  Render));
        }

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPathProperty;

        public bool IsHiddenVisible
        {
            get { return (bool)GetValue(IsHiddenVisibleProperty); }
            set { SetValue(IsHiddenVisibleProperty, value); }
        }

        public IEnumerable<string> FilteredExtensions
        {
            get;
            set;
        }

        // Using a DependencyProperty as the backing store for IsHiddenVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHiddenVisibleProperty;

        private static void Render(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RenderFileList(string path)
        {
            var items = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                if (IsHiddenVisible)
                {
                    items.AddRange(Directory.GetDirectories(path));
                    if (FilteredExtensions != null && FilteredExtensions.Any())
                        return;
                    else
                    {
                        var dir = new DirectoryInfo(path);
                        var files = from i in dir.GetFiles()
                                    where FilteredExtensions.Contains(i.Extension)
                                    select i.FullName;
                    }
                }
                else
                {
                    var dir = new DirectoryInfo(path);
                    var folders = from i in dir.GetDirectories()
                                  where !i.Attributes.HasFlag(FileAttributes.Hidden)
                                  select i.FullName;
                    items.AddRange(folders);

                    if (FilteredExtensions != null && FilteredExtensions.Any())
                    {
                        var files = from i in dir.GetFiles()
                                    where !i.Attributes.HasFlag(FileAttributes.Hidden)
                                    && FilteredExtensions.Contains(i.Extension)
                                    select i.FullName;
                        items.AddRange(files);
                    }
                    else
                    {
                        var files = from i in dir.GetFiles()
                                    where !i.Attributes.HasFlag(FileAttributes.Hidden)
                                    select i.FullName;
                        items.AddRange(files);
                    }
                }
               ItemsSource = null;
               ItemsSource = items;
            }
            catch (Exception)
            {
                ItemsSource = null;
            }

        }
    }
}
