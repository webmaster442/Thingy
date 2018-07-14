using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Thingy.API;
using Thingy.CoreModules.MnbServiceReference;
using Thingy.CoreModules.Models;

namespace Thingy.CoreModules.ViewModels
{
    public class CurrencyConverterViewModel : ViewModel
    {
        private decimal _input;
        private decimal _output;
        private DateTime _lastupdate;
        private Visibility _UpdateVisibility;
        private int _selectedInputIndex;
        private int _selectedOutputIndex;
        private IApplication _app;

        public DelegateCommand UpdateCommand { get; private set; }

        public CurrencyConverterViewModel(IApplication app)
        {
            _app = app;
            CurrencyTypes = new ObservableCollection<string>();
            CurrencyRates = new ObservableCollection<CurrencyRate>();
            UpdateVisibility = Visibility.Collapsed;
            UpdateCommand = Command.CreateCommand(Update);
        }

        private async void Update()
        {
            try
            {
                UpdateVisibility = Visibility.Visible;
                var data = await UpdateRates();
                data.Add(new CurrencyRate
                {
                    CurrencyCode = "HUF",
                    Unit = 1,
                    ValueInForint = 1
                });
                CurrencyRates.Clear();
                CurrencyRates.AddRange(data.OrderBy(d => d.CurrencyCode));
                OnPropertyChanged(() => CurrencyRates);

                var types = from rate in CurrencyRates
                            orderby rate.CurrencyCode ascending
                            select rate.CurrencyCode;
                CurrencyTypes.Clear();
                CurrencyTypes.AddRange(types);
                OnPropertyChanged(() => CurrencyTypes);
                _selectedInputIndex = CurrencyTypes.IndexOf("EUR");
                _selectedOutputIndex = CurrencyTypes.IndexOf("HUF");
                OnPropertyChanged(() => SelectedInputIndex);
                OnPropertyChanged(() => SelectedOutputIndex);
                LastUpdate = DateTime.Now;
                Input = 1;
                UpdateVisibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LastUpdate = DateTime.Now;
                CurrencyRates.Clear();
                CurrencyTypes.Clear();
                _app.Log.Error(ex);
                await _app.ShowMessageBox("Error", "Error calling webservice", DialogButtons.Ok);
                UpdateVisibility = Visibility.Collapsed;
            }
        }

        private async Task<IList<CurrencyRate>> UpdateRates()
        {
            using (MNBArfolyamServiceSoapClient client = new MNBArfolyamServiceSoapClient(new BasicHttpBinding(), new EndpointAddress("http://www.mnb.hu/arfolyamok.asmx")))
            {
                var respone = await client.GetCurrentExchangeRatesAsync(new GetCurrentExchangeRatesRequestBody());
                var current = respone.GetCurrentExchangeRatesResponse1.GetCurrentExchangeRatesResult;
                XDocument xml = XDocument.Parse(current);
                List<CurrencyRate> result = new List<CurrencyRate>();
                foreach (var rate in xml.Descendants("Rate"))
                {
                    var parsed = new CurrencyRate
                    {
                        CurrencyCode = rate.Attribute("curr").Value,
                        Unit = Convert.ToDecimal(rate.Attribute("unit").Value, new CultureInfo("hu-HU")),
                        ValueInForint = Convert.ToDecimal(rate.Value, new CultureInfo("hu-HU")),
                    };
                    result.Add(parsed);
                }
                return result;
            }
        }

        private async void ConvertRate(decimal value)
        {
            try
            {
                var inputtype = CurrencyTypes[SelectedInputIndex];
                var outputtype = CurrencyTypes[SelectedOutputIndex];

                var inputinforints = (from rate in CurrencyRates
                                      where rate.CurrencyCode == inputtype
                                      select rate.ValueInForint / rate.Unit).FirstOrDefault();

                var outputinforints = inputinforints * value;

                var outcurrencyinforints = (from rate in CurrencyRates
                                            where rate.CurrencyCode == outputtype
                                            select rate.ValueInForint / rate.Unit).FirstOrDefault();

                var output = outputinforints / outcurrencyinforints;
                Output = output;
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
                await _app.ShowMessageBox("Error", "Error on conversion.", DialogButtons.Ok);
                Output = 0;
            }
        }

        public ObservableCollection<CurrencyRate> CurrencyRates
        {
            get;
            private set;
        }

        public ObservableCollection<string> CurrencyTypes
        {
            get;
            private set;
        }

        public decimal Input
        {
            get { return _input; }
            set
            {
                SetValue(ref _input, value);
                ConvertRate(value);
            }
        }

        public decimal Output
        {
            get { return _output; }
            set { SetValue(ref _output, value); }
        }

        public DateTime LastUpdate
        {
            get { return _lastupdate; }
            set { SetValue(ref _lastupdate, value); }
        }

        public Visibility UpdateVisibility
        {
            get { return _UpdateVisibility; }
            set { SetValue(ref _UpdateVisibility, value); }
        }

        public int SelectedInputIndex
        {
            get { return _selectedInputIndex; }
            set
            {
                SetValue(ref _selectedInputIndex, value);
                if (value > 0) ConvertRate(_input);
            }
        }

        public int SelectedOutputIndex
        {
            get { return _selectedOutputIndex; }
            set
            {
                SetValue(ref _selectedOutputIndex, value);
                if (value > 0) ConvertRate(_input);
            }
        }
    }
}
