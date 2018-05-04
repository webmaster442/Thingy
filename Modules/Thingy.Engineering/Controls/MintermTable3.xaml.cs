using AppLib.WPF.Extensions;
using System.Collections.Generic;
using System.Windows.Controls;
using Thingy.Engineering.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    /// <summary>
    /// Interaction logic for MintermTable3.xaml
    /// </summary>
    public partial class MintermTable3 : UserControl, IMintermTable
    {
        public MintermTable3()
        {
            InitializeComponent();
        }

        public LogicItem[] GetSelected()
        {
            var ret = new List<LogicItem>();
            foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm3x))
            {
                ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 3, item.Value));
            }
            return ret.ToArray();
        }

        public void SetSelected(LogicItem[] vals)
        {
            MintermTableHelpers.SetMintermTableValues(Minterm3x, vals);
        }

        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm3x);
        }
  
        public void SetAll(bool? value)
        {
            var items = new List<LogicItem>();
            for (int i = 0; i < 8; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 3, value);
                items.Add(lo);
            }
            SetSelected(items.ToArray());
        }

        public void SwapVarnames()
        {
            foreach (var tb in this.FindChildren<TextBlock>())
            {
                switch (tb.Text)
                {
                    case "A":
                        tb.Text = "C";
                        break;
                    case "B":
                        tb.Text = "B";
                        break;
                    case "C":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
