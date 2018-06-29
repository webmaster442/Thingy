using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.FileBrowser.Controls
{
    internal class DirectoryTree: TreeView, IFileSystemControl
    {
        private object _dummyNode = null;
        private bool _lock;
        private string _lastdriveLetter;

        static DirectoryTree()
        {
            SelectedPathProperty = DependencyProperty.Register("SelectedPath",
                                                                typeof(string),
                                                                typeof(DirectoryTree),
                                                                new FrameworkPropertyMetadata("",
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                Render));

            IsHiddenVisibleProperty = DependencyProperty.Register("IsHiddenVisible", 
                                                                  typeof(bool), 
                                                                  typeof(DirectoryTree),
                                                                  new FrameworkPropertyMetadata(false,
                                                                  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                  HiddenChanged));
        }



        public DirectoryTree()
        {
            MouseDoubleClick += DirectoryTree_MouseDoubleClick;
            _lastdriveLetter = null;
        }

        private void DirectoryTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(SelectedItem is TreeViewItem item)) return;
            SelectedPath = item.Tag.ToString();
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

        // Using a DependencyProperty as the backing store for IsHiddenVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHiddenVisibleProperty;

        public event EventHandler<string> OnNavigationException;

        private static void HiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectoryTree sender = d as DirectoryTree;
            if (sender._lock) return;
            sender.Render(true);
        }

        private static void Render(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectoryTree sender = d as DirectoryTree;
            if (sender._lock) return;
            sender.Render();
        }

        private void Render(bool HiddenHasChanged = false)
        {
            if (string.IsNullOrEmpty(SelectedPath)) return;

            if (SelectedPath != FileListView.HomePath)
            {
                var parts = SelectedPath.Split('\\', '/');
                var root = Directory.GetDirectoryRoot(SelectedPath);
                if (root != _lastdriveLetter || HiddenHasChanged)
                {
                    Items.Clear();
                    RenderFolderView($"{parts[0]}\\");
                    _lastdriveLetter = root;
                }
                SelectNodePath(SelectedPath);
            }
            else
            {
                Items.Clear();
            }
        }

        private void RenderFolderView(string driveLetter)
        {
            if (string.IsNullOrEmpty(driveLetter)) return;

            try
            {
                string[] directories = null;

                if (!IsHiddenVisible)
                {
                    var dir = new DirectoryInfo(Directory.GetDirectoryRoot(driveLetter));

                    

                    var folders = from i in dir.GetDirectories()
                                  where !i.Attributes.HasFlag(FileAttributes.Hidden)
                                  select i.FullName;
                    directories = folders.ToArray();
                }
                else
                {
                    directories = Directory.GetDirectories(driveLetter);
                }
                foreach (string s in directories)
                {
                    var folder = new TreeViewItem
                    {
                        Header = Path.GetFileName(s),
                        Tag = s,
                        FontWeight = FontWeights.Normal
                    };
                    folder.Items.Add(_dummyNode);
                    folder.Expanded += Folder_Expanded;
                    Items.Add(folder);
                }
            }
            catch (Exception ex)
            {
                OnNavigationException?.Invoke(this, ex.Message);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == _dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem
                        {
                            Header = s.Substring(s.LastIndexOf("\\") + 1),
                            Tag = s,
                            FontWeight = FontWeights.Normal
                        };
                        subitem.Items.Add(_dummyNode);
                        subitem.Expanded += Folder_Expanded;
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception ex)
                {
                    OnNavigationException?.Invoke(this, ex.Message);
                }
            }
        }

        private TreeViewItem FromID(string itemId, TreeViewItem rootNode)
        {
            if (rootNode == null)
            {
                var q = Items.OfType<TreeViewItem>().FirstOrDefault(node => node.Tag.Equals(itemId));
                if (q != null) return q;
                else
                {
                    var q2 = from i in Items.OfType<TreeViewItem>()
                             where i.IsExpanded == true ||
                             itemId.StartsWith(i.Tag.ToString())
                             select i;

                    foreach (var node in q2)
                    {
                        node.IsExpanded = true;
                        var result = FromID(itemId, node);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                    return null;
                }
            }
            else
            {
                var relavantnodes = from TreeViewItem i in rootNode.Items
                                    where itemId.StartsWith(i.Tag.ToString())
                                    select i;

                foreach (TreeViewItem node in relavantnodes)
                {
                    if (node == null) continue;
                    if (node.Tag.Equals(itemId)) return node;
                    node.IsExpanded = true;
                    var next = FromID(itemId, node);
                    if (next != null) return next;
                }
                return null;
            }
        }

        private void SelectNodePath(string path)
        {
            var node = FromID(path, null);
            if (node != null)
            {
                node.BringIntoView();
                node.IsExpanded = true;
            }
        }

        private void Folders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView)sender;
            TreeViewItem temp = ((TreeViewItem)tree.SelectedItem);

            if (temp == null) return;
            _lock = true;
            SelectedPath = temp.Tag.ToString();
            _lock = false;
        }
    }
}
