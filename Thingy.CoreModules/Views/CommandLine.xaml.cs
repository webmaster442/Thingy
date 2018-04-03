﻿using System.Windows.Controls;
using Thingy.CoreModules.ViewModels;

namespace Thingy.CoreModules.Views
{
    /// <summary>
    /// Interaction logic for CommandLine.xaml
    /// </summary>
    public partial class CommandLine : UserControl, ICommandLineView
    {
        public CommandLine()
        {
            InitializeComponent();
        }

        public CommandLineViewModel ViewModel
        {
            get { return DataContext as CommandLineViewModel; }
        }

        public void Close()
        {
            ViewModel?.ClosingCommand?.Execute(null);
        }

        public TextBox GetTextBox()
        {
            return Console;
        }
    }
}
