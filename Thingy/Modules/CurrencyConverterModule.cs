﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Thingy.Modules
{
    public class CurrencyConverterModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Currency Converter"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-currency-exchange-96.png")); }
        }

        public override UserControl RunModule()
        {
            return new CurrencyConvert.CurrencyConverter
            {
                DataContext = new CurrencyConvert.CurrencyConverterModel()
            };
        }
    }
}