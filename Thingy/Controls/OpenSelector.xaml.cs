using AppLib.Common.Extensions;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Thingy.API;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for OpenSelector.xaml
    /// </summary>
    public partial class OpenSelector : CustomDialog
    {
        private IApplication _app;

        public ObservableCollection<OpenSelectorItem> ItemsCollection { get; set; }
        private IList<IModule> _modules;

        public OpenSelector()
        {
            InitializeComponent();
            ItemsCollection = new ObservableCollection<OpenSelectorItem>();
        }

        public IList<IModule> InputModules
        {
            get { return _modules; }
            set
            {
                _modules = value;
                CreateButtonItems();
            }
        }

        public IModule SelectedModule
        {
            get
            {
                var selected = ModuleList.SelectedIndex;
                if (selected < 0) return null;
                return _modules[selected];
            }
        }

        private void CreateButtonItems()
        {
            var query = from module in _modules
                        select new OpenSelectorItem
                        {
                            Icon = module.Icon,
                            Text = $"Open supported files with {module.ModuleName}"
                        };

            ItemsCollection.UpdateWith(query);
        }

        public OpenSelector(IApplication app): this()
        {
            _app = app;
        }

        private async void PART_NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            await _app.HideMessageBox(this);
        }
    }
}
