using AppLib.Maths;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Numerics;
using System.Speech.Synthesis;
using System.Windows;
using Thingy.API;
using Thingy.CalculatorCore;

namespace Thingy.Calculator.Dialogs
{
    /// <summary>
    /// Interaction logic for NumberToTextWindow.xaml
    /// </summary>
    public partial class NumberToTexMessageBox : CustomDialog, IDisposable
    {
        private SpeechSynthesizer _synthesizer;
        private IApplication _app;

        public NumberToTexMessageBox(IApplication application)
        {
            InitializeComponent();
            _app = application;
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SpeakCompleted += _synthesizer_SpeakCompleted;
        }

        ~NumberToTexMessageBox()
        {
            Dispose(true);
        }

        public void SetDisplay(object o)
        {
            if (o is Complex)
            {
                var par = (Complex)o;
                TbText.Text = string.Format("Real part: {0}, Imaginary part: {1}",
                                            NumberToText.NumberText(par.Real),
                                            NumberToText.NumberText(par.Imaginary));
            }
            else if (o is Fraction)
            {
                var fr = (Fraction)o;
                TbText.Text = string.Format("{0} over {1}",
                                            NumberToText.NumberText(fr.Numerator),
                                            NumberToText.NumberText(fr.Denominator));
            }
            else
            {
                try
                {
                    var d = Convert.ToDouble(o);
                    TbText.Text = NumberToText.NumberText(d);
                }
                catch (Exception) { TbText.Text = "Not a number"; }
                
            }
        }

        private void BtnSayIT_Click(object sender, RoutedEventArgs e)
        {
            _synthesizer.Volume = Convert.ToInt32(SliderVolume.Value);
            _synthesizer.Rate = Convert.ToInt32(SliderSpeed.Value);
            BtnSayIT.IsEnabled = false;
            _synthesizer.SpeakAsync(TbText.Text);
        }

        private void _synthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            BtnSayIT.IsEnabled = true;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TbText.Text);
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _app.HideMessageBox(this);
        }

        protected virtual void Dispose(bool native)
        {
            if (_synthesizer != null)
            {
                _synthesizer.Dispose();
                _synthesizer = null;
            }
        }
            
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
