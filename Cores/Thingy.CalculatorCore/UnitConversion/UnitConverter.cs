using System.Collections.Generic;
using System.Linq;

namespace Thingy.CalculatorCore.UnitConversion
{
    public class UnitConverter
    {
        private UnitProvider _provider;

        public UnitConverter()
        {
            _provider = new UnitProvider();
        }

        private double ConvertUnitToCategoryStandard(Unit unit, double value)
        {
            double res = 0;
            if (unit.Offset != 0) value -= unit.Offset;
            switch (unit.Action)
            {
                case Actions.Multiply:
                    res = value / unit.Ratio;
                    break;
                case Actions.Add:
                    res = value - unit.Ratio;
                    break;
                case Actions.Divide:
                    res = value * unit.Ratio;
                    break;
                case Actions.Subtract:
                    res = value + unit.Ratio;
                    break;
                default:
                    return value;
            }
            return res;
        }

        private double ConvertCategoryStandardToUnit(Unit u, double stdval)
        {
            double res = 0;
            switch (u.Action)
            {
                case Actions.Multiply:
                    res = stdval * u.Ratio;
                    break;
                case Actions.Divide:
                    res = stdval / u.Ratio;
                    break;
                case Actions.Add:
                    res = stdval + u.Ratio;
                    break;
                case Actions.Subtract:
                    res = stdval - u.Ratio;
                    break;
                default:
                    return stdval;
            }
            if (u.Offset != 0) res += u.Offset;
            return res;
        }

        public double Convert(string sourceUnitName, string destinationUnitName, string unitCategoryName, double value)
        {
            Unit source, destination;

            var category = _provider[unitCategoryName];

            source = (from i in category where string.Compare(i.Name, sourceUnitName, true) == 0 select i).FirstOrDefault();
            destination = (from i in category where string.Compare(i.Name, destinationUnitName, true) == 0 select i).FirstOrDefault();

            if (source == null || destination == null) return double.NaN;

            double categoryStandard = ConvertUnitToCategoryStandard(source, value);
            return ConvertCategoryStandardToUnit(destination, categoryStandard);
        }

        public IEnumerable<string> Categories
        {
            get { return _provider.Keys; }
        }

        public IEnumerable<string> GetUnitsForCategory(string categoryName)
        {
            return _provider[categoryName].Select(unit => unit.Name);
        }
    }
}
