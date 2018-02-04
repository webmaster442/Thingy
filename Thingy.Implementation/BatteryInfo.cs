using System;
using System.Management;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Thingy.Implementation
{
    public class BatteryInfo: Image
    {
        public void UpdateBatteryInfo()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");

                StringBuilder tooltip = new StringBuilder();

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    tooltip.AppendLine("-----------------------------------");
                    tooltip.AppendLine("Battery Info");
                    tooltip.AppendLine("-----------------------------------");
                    tooltip.AppendFormat("BatteryStatus: {0}\n", DecodeStatus(queryObj["BatteryStatus"]));
                    tooltip.AppendFormat("Chemistry: {0}\n", DecodeChemistry(queryObj["Chemistry"]));
                    tooltip.AppendFormat("EstimatedChargeRemaining: {0}\n", queryObj["EstimatedChargeRemaining"]);
                    tooltip.AppendFormat("EstimatedRunTime: {0}\n", DecodeRuntime(queryObj["EstimatedRunTime"]));
                    SetSource(queryObj["EstimatedChargeRemaining"], queryObj["BatteryStatus"]);
                    ToolTip = CreateTooltip(tooltip, queryObj["EstimatedChargeRemaining"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private object CreateTooltip(StringBuilder tooltip, object v)
        {
            StackPanel sp = new StackPanel();
            TextBlock text = new TextBlock
            {
                Text = tooltip.ToString()
            };
            sp.Children.Add(text);
            ProgressBar pb = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = Convert.ToInt32(v)
            };
            sp.Children.Add(pb);
            return sp;
        }

        private void SetSource(object remaining, object status)
        {
            int charge = Convert.ToInt32(remaining);
            int stat = Convert.ToInt32(status);

            if (stat == 3)
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-charging-battery.png"));
            }
            else
            {
                if (charge >= 0 && charge <= 25)
                    Source = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-low-battery.png"));
                else if (charge >= 26 && charge <= 50)
                    Source = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-battery-level.png"));
                else if (charge >= 51 && charge <= 75)
                    Source = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-charged-battery.png"));
                else if (charge >= 76 && charge <= 100)
                    Source = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-full-battery.png"));
            }
        }

        private string DecodeRuntime(object runtime)
        {
            uint time = Convert.ToUInt32(runtime);
            if (time >= 71582700)
                return "∞";
            else
                return TimeSpan.FromMinutes(time).ToString();
        }

        private string DecodeStatus(object status)
        {
            int stat = Convert.ToInt32(status);
            switch (stat)
            {
                case 1:
                    return "The battery is discharging.";
                case 2:
                    return "The system has access to AC so no battery is being discharged.\r\nHowever, the battery is not necessarily charging.";
                case 3:
                    return "Fully Charged";
                case 4:
                    return "Low";
                case 5:
                    return "Critical";
                case 6:
                    return "Charging";
                case 7:
                    return "Charging and High";
                case 8:
                    return "Charging and Low";
                case 9:
                    return "Charging and Critical";
                case 11:
                    return "Partially Charged ";
                default:
                    return "Undefined";
            }
        }

        private string DecodeChemistry(object chem)
        {
            int ch = Convert.ToInt32(chem);
            switch (ch)
            {
                case 1:
                    return "Other";
                case 3:
                    return "Lead Acid";
                case 4:
                    return "Nickel Cadmium";
                case 5:
                    return "Nickel Metal Hydride";
                case 6:
                    return "Lithium-ion";
                case 7:
                    return "Zinc air";
                case 8:
                    return "Lithium Polymer";
                default:
                    return "Unknown";
            }
        }
    }
}
