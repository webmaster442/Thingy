using System.Collections.Generic;
using System.Windows.Controls;
using AppLib.WPF.Extensions;
using Thingy.Engineering.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    /// <summary>
    /// Interaction logic for MintermTable.xaml
    /// </summary>
    public partial class MintermTable4 : UserControl, IMintermTable
    {
        public MintermTable4()
        {
            InitializeComponent();
        }

        public LogicItem[] GetSelected()
        {
            var ret = new List<LogicItem>();
            foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm4x))
            {
                ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 4, item.Value));
            }
            return ret.ToArray();
        }

        public void SetSelected(LogicItem[] vals)
        {
            MintermTableHelpers.SetMintermTableValues(Minterm4x, vals);
        }

        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm4x);
        }

        public void SetAll(bool? value)
        {
            var items = new List<LogicItem>();
            for (int i = 0; i < 16; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 4, value);
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
                        tb.Text = "D";
                        break;
                    case "B":
                        tb.Text = "C";
                        break;
                    case "C":
                        tb.Text = "B";
                        break;
                    case "D":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
