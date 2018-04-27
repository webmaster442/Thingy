using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;
using Thingy.API;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    public partial class ModalDialogReal : MetroWindow, IModalDialog
    {
        private DialogButtons _dialogbuttons;

        private INotifyDataErrorInfo ValidatableContent;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ConfigureButtons()
        {
            switch (DialogButtons)
            {
                case DialogButtons.None:
                    OkButton.Visibility = Visibility.Collapsed;
                    CancelButton.Visibility = Visibility.Collapsed;
                    break;

                case DialogButtons.Ok:
                    OkButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Collapsed;
                    break;

                case DialogButtons.OkCancel:
                    OkButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    break;

                case DialogButtons.YesNo:
                    OkButton.Content = "Yes";
                    CancelButton.Content = "No";
                    OkButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ErrorHandler(object sender, DataErrorsChangedEventArgs e)
        {
            OkButton.IsEnabled = !ValidatableContent.HasErrors;
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

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public static readonly DependencyProperty DialogContentProperty =
                                                                    DependencyProperty.Register("DailogContent", typeof(object), typeof(ModalDialogReal));

        public ModalDialogReal()
        {
            InitializeComponent();
        }

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
                    OkButton.IsEnabled = true;
                    ErrorLabel.Visibility = Visibility.Collapsed;
                }
            }
        }

        public DialogButtons DialogButtons
        {
            get { return _dialogbuttons; }
            set
            {
                _dialogbuttons = value;
                ConfigureButtons();
            }
        }
    }
}
