using MahApps.Metro.SimpleChildWindow;
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
    public partial class ModalDialog : ChildWindow
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
            Ok.IsEnabled = !ValidatableContent.HasErrors;
        }

        public ModalDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
