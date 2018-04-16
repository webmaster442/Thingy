using System;
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

namespace Thingy.GitBash.Dialogs
{
    /// <summary>
    /// Interaction logic for StringInputDialog.xaml
    /// </summary>
    public partial class StringInputDialog : UserControl
    {
        public StringInputDialog()
        {
            InitializeComponent();
        }

        public string Description
        {
            get { return TbDescription.Text; }
            set { TbDescription.Text = value; }
        }

        public string Input
        {
            get { return TbInput.Text; }
            set { TbInput.Text = value; }
        }
    }
}
