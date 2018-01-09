using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thingy.CalculatorCore.Constants;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for CalculatorConstants.xaml
    /// </summary>
    public partial class CalculatorConstants : UserControl
    {
        private ConstantDB _constantDB;

        public CalculatorConstants()
        {
            InitializeComponent();
            _constantDB = new ConstantDB();
            ConstantCategories = _constantDB.Categories;
            VisibleConstants = _constantDB.GetCategory(ConstantCategories.First());
        }

        public static readonly DependencyProperty VisibleConstantsProperty =
            DependencyProperty.Register("VisibleConstants", typeof(IEnumerable<Constant>), typeof(CalculatorConstants));

        public IEnumerable<Constant> VisibleConstants
        {
            get { return (IEnumerable<Constant>)GetValue(VisibleConstantsProperty); }
            set { SetValue(VisibleConstantsProperty, value); }
        }

        public static readonly DependencyProperty ConstantCategoriesProperty =
            DependencyProperty.Register("ConstantCategories", typeof(IEnumerable<string>), typeof(CalculatorConstants));

        public IEnumerable<string> ConstantCategories
        {
            get { return (IEnumerable<string>)GetValue(ConstantCategoriesProperty); }
            set { SetValue(ConstantCategoriesProperty, value); }
        }
    }
}
