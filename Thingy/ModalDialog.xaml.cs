using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    public partial class ModalDialog : Window
    {
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DailogContent", typeof(object), typeof(ModalDialog));

        private INotifyDataErrorInfo ValidatableContent;

        public object DailogContent
        {
            get { return GetValue(DialogContentProperty); }
            set
            {
                SetValue(DialogContentProperty, value);
                var errorinfo = GetDialogContentModell(value);
                if (errorinfo != null)
                {
                    if (ValidatableContent != null)
                        ValidatableContent.ErrorsChanged -= ErrorHandler;
                    ValidatableContent = errorinfo;
                    ValidatableContent.ErrorsChanged += ErrorHandler;
                    ErrorHandler(null, null);
                }
                else
                {
                    if (ValidatableContent != null)
                        ValidatableContent.ErrorsChanged -= ErrorHandler;
                    Ok.IsEnabled = true;
                    ErrorLabel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private INotifyDataErrorInfo GetDialogContentModell(object content)
        {
            if (content is FrameworkElement element && element.DataContext != null)
            {
                if (element.DataContext is INotifyDataErrorInfo model)
                    return model;
            }
            return null;
        }

        private void ErrorHandler(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorLabel.ToolTip = BuildErrorString(ValidatableContent.GetErrors(""));
            if (ValidatableContent.HasErrors)
            {
                Ok.IsEnabled = false;
                ErrorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                Ok.IsEnabled = true;
                ErrorLabel.Visibility = Visibility.Collapsed;
            }
        }

        private object BuildErrorString(IEnumerable enumerable)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in enumerable)
            {
                sb.AppendLine(error);
            }
            return sb.ToString();
        }

        public ModalDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
