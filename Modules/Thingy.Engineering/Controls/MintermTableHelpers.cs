using AppLib.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Thingy.Engineering.Domain.LogicMinimizer;

namespace Thingy.Engineering.Controls
{
    internal static class MintermTableHelpers
    {
        public static Dictionary<int, bool?> GetMintermTableValues(Grid Minterm)
        {
            var ret = new Dictionary<int, bool?>();
            var checkboxes = Minterm.FindChildren<CheckBox>();
            foreach (var checkbox in checkboxes)
            {
                ret.Add(Convert.ToInt32(checkbox.Content), checkbox.IsChecked);
            }
            return ret;
        }

        public static void SetMintermTableValues(Grid Minterm, IEnumerable<LogicItem> items)
        {
            var checkboxes = Minterm.FindChildren<CheckBox>();
            foreach (var item in items)
            {
                var checkbox = (from chb in checkboxes
                                where chb.Content.ToString() == item.Index.ToString()
                                select chb).FirstOrDefault();

                if (checkbox != null) checkbox.IsChecked = item.Checked;
            }
        }

        public static void ClearMintermtable(Grid Minterm)
        {
            var checkboxes = Minterm.FindChildren<CheckBox>();
            foreach (var checkbox in checkboxes)
            {
                checkbox.IsChecked = false;
            }
        }
    }


}
