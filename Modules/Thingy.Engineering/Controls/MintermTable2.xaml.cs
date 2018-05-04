using AppLib.WPF.Extensions;
using System.Collections.Generic;
using System.Windows.Controls;
using Thingy.Engineering.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    /// <summary>
    /// Interaction logic for MintermTable2.xaml
    /// </summary>
    public partial class MintermTable2 : UserControl, IMintermTable
    {
        public MintermTable2()
        {
            InitializeComponent();
        }

        public void ClearInput()
        {
            MintermTableHelpers.ClearMintermtable(Minterm2x);
        }

        public LogicItem[] GetSelected()
        {
            var ret = new List<LogicItem>();
            foreach (var item in MintermTableHelpers.GetMintermTableValues(Minterm2x))
            {
                ret.Add(LogicItem.CreateFromMintermIndex(item.Key, 2, item.Value));
            }
            return ret.ToArray();
        }

        public void SetSelected(LogicItem[] vals)
        {
            MintermTableHelpers.SetMintermTableValues(Minterm2x, vals);
        }

        public void SetAll(bool? value)
        {
            var items = new List<LogicItem>();
            for (int i=0; i<3; i++)
            {
                LogicItem lo = LogicItem.CreateFromMintermIndex(i, 2, value);
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
                        tb.Text = "B";
                        break;
                    case "B":
                        tb.Text = "A";
                        break;
                }
            }
        }
    }
}
