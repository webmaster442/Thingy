using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Thingy.CurrencyConvert.MnbServiceReference;

namespace Thingy.CurrencyConvert
{
    public class CurrencyConverterModel : ViewModel
    {
        private decimal _input;
        private decimal _output;
        private DateTime _lastupdate;
        private Visibility _UpdateVisibility;
        private int _selectedInputIndex;
        private int _selectedOutputIndex;

        public DelegateCommand UpdateCommand { get; private set; }

        public CurrencyConverterModel()
        {
            CurrencyTypes = new ObservableCollection<string>();
            CurrencyRates = new ObservableCollection<Rate>();
            UpdateVisibility = Visibility.Collapsed;
            UpdateCommand = Command.ToCommand(Update);
        }

        private async void Update()
        {
            try
            {
                UpdateVisibility = Visibility.Visible;
                var data = await UpdateRates();
                data.Add(new Rate
                {
                    Curr = "HUF",
                    Unit = 1,
                    ValueInForint = 1
                });
                CurrencyRates.Clear();
                CurrencyRates.AddRange(data.OrderBy(d => d.Curr));
                OnPropertyChanged(() => CurrencyRates);

                var types = from rate in CurrencyRates
                            orderby rate.Curr ascending
                            select rate.Curr;
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
                MessageBox.Show("Error calling webservice:\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateVisibility = Visibility.Collapsed;
            }
        }

        private async Task<IList<Rate>> UpdateRates()
        {
            using (MNBArfolyamServiceSoapClient client = new MNBArfolyamServiceSoapClient())
            {
                var respone = await client.GetCurrentExchangeRatesAsync(new GetCurrentExchangeRatesRequest());
                var current = respone.GetCurrentExchangeRatesResponse1.GetCurrentExchangeRatesResult;
                XDocument xml = XDocument.Parse(current);
                List<Rate> result = new List<Rate>();
                foreach (var rate in xml.Descendants("Rate"))
                {
                    var parsed = new Rate
                    {
                        Curr = rate.Attribute("curr").Value,
                        Unit = Convert.ToDecimal(rate.Attribute("unit").Value),
                        ValueInForint = Convert.ToDecimal(rate.Value),
                    };
                    result.Add(parsed);
                }
                return result;
            }
        }

        private void ConvertRate(decimal value)
        {
            try
            {
                var inputtype = CurrencyTypes[SelectedInputIndex];
                var outputtype = CurrencyTypes[SelectedOutputIndex];

                var inputinforints = (from rate in CurrencyRates
                                      where rate.Curr == inputtype
                                      select rate.ValueInForint / rate.Unit).FirstOrDefault();

                var outputinforints = inputinforints * value;

                var outcurrencyinforints = (from rate in CurrencyRates
                                            where rate.Curr == outputtype
                                            select rate.ValueInForint / rate.Unit).FirstOrDefault();

                var output = outputinforints / outcurrencyinforints;
                Output = output;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on conversion:\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Output = 0;
            }
        }

        public ObservableCollection<Rate> CurrencyRates
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
