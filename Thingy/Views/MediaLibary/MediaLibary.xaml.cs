﻿using System;
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

namespace Thingy.Views.MediaLibary
{
    /// <summary>
    /// Interaction logic for MediaLibary.xaml
    /// </summary>
    public partial class MediaLibary : UserControl
    {
        public MediaLibary()
        {
            InitializeComponent();
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = DataTree.SelectedItem as string;
            var tag = (e.OriginalSource as TextBlock)?.Tag;
            if (selected != null && tag != null)
            {
                (DataContext as ViewModels.MediaLibary.MediaLibaryViewModel)?.CategoryQueryCommand.Execute(new string[] { selected, tag.ToString() });
            }

        }
    }
}
