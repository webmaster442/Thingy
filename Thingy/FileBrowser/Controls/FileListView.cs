using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public FileListView()
        {
            MouseDoubleClick += FileListView_MouseDoubleClick;
        }

        public event EventHandler<string> FileDoubleClick;

        private void FileListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItem != null)
            {
                var item = SelectedItem as string;
                if (Directory.Exists(item))
                {
                    SelectedPath = item;
                }
                else if (File.Exists(item))
                {
                    FileDoubleClick?.Invoke(this, item);
                }
            }
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

        public event EventHandler<string> OnNavigationException;

        private static void Render(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FileListView sender = d as FileListView;
            sender.RenderFileList(sender.SelectedPath);
        }

        private void RenderFileList(string path)
        {
            if (path == HomePath)
            {
                ItemsSource = RenderHome();
                return;
            }

            var items = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                var dir = new DirectoryInfo(path);

                if (IsHiddenVisible)
                {
                    items.AddRange(Directory.GetDirectories(path));
                    if (FilteredExtensions != null && FilteredExtensions.Any())
                    {

                        var files = from i in dir.GetFiles()
                                    where FilteredExtensions.Contains(i.Extension)
                                    select i.FullName;
                        items.AddRange(files);
                    }
                    else
                    {
                        var files = from i in dir.GetFiles()
                                    select i.FullName;
                        items.AddRange(files);
                    }
                }
                else
                {

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
            catch (Exception ex)
            {
                ItemsSource = null;
                OnNavigationException?.Invoke(this, ex.Message);
            }

        }

        private IEnumerable RenderHome()
        {
            try
            {
                var items = new List<string>();

                var drives = from drive in DriveInfo.GetDrives()
                             where drive.IsReady == true
                             select drive.Name;

                items.AddRange(drives);
                items.AddRange(new string[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)
                });

                return items;
            }
            catch (Exception ex)
            {
                OnNavigationException?.Invoke(this, ex.Message);
                return null;
            }
        }

        public void GoHome()
        {
            SelectedPath = HomePath;
        }

        public const string HomePath = @"HOME:\";
    }
}
