using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thingy.CalculatorCore.Constants;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for CalculatorConstants.xaml
    /// </summary>
    public partial class CalculatorConstants : UserControl
    {
        public CalculatorConstants()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ConstantDBProperty =
            DependencyProperty.Register("ConstantDB", typeof(IConstantDB), typeof(CalculatorConstants), new PropertyMetadata(null, DBchanged));

        public IConstantDB ConstantDB
        {
            get { return (IConstantDB)GetValue(ConstantDBProperty); }
            set { SetValue(ConstantDBProperty, value); }
        }

        private static void DBchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CalculatorConstants sender)
            {
                if (e.NewValue is ConstantDB db)
                {
                    sender.VisibleConstants = db.GetCategory(db.Categories.First());
                }
            }
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

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(CalculatorConstants), new FrameworkPropertyMetadata(null));


        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty InsertCommandProperty =
            DependencyProperty.Register("InsertCommand", typeof(ICommand), typeof(CalculatorConstants), new FrameworkPropertyMetadata(null));


        public ICommand InsertCommand
        {
            get { return (ICommand)GetValue(InsertCommandProperty); }
            set { SetValue(InsertCommandProperty, value); }
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Categories.SelectedItem as string;
            VisibleConstants = ConstantDB?.GetCategory(selected);
        }
    }
}
