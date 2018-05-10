using System.Collections.Generic;
using System.Windows.Controls;
using AppLib.WPF.Extensions;
using Thingy.Engineering.Domain.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    /// <summary>
    /// Interaction logic for MintermTable5.xaml
    /// </summary>
    public partial class MintermTable5 : UserControl, IMintermTable
    {
        public MintermTable5()
        {
            InitializeComponent();
        }

        public LogicItem[] GetSelected()
        {
            var ret = new List<LogicItem>();
            foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm5x))
            {
                ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 5, item.Value));
            }
            return ret.ToArray();
        }

        public void SetSelected(LogicItem[] vals)
        {
            MintermTableHelpers.SetMintermTableValues(Minterm5x, vals);
        }

        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm5x);
        }

        public void SetAll(bool? value)
        {
            var items = new List<LogicItem>();
            for (int i = 0; i < 32; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 5, value);
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
                        tb.Text = "E";
                        break;
                    case "B":
                        tb.Text = "D";
                        break;
                    case "C":
                        tb.Text = "C";
                        break;
                    case "D":
                        tb.Text = "B";
                        break;
                    case "E":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
