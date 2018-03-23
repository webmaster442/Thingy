using System.Collections.Generic;

namespace Thingy.CalculatorCore.UnitConversion
{
    public class UnitProvider: Dictionary<string, IEnumerable<Unit>>
    {
        public UnitProvider()
        {
            Add("Distance", new Unit[]
            {
                new Unit("Meter", 1, Actions.None),
                new Unit("Feet", 3.280839895, Actions.Multiply),
                new Unit("Mil", 39370.0787401575, Actions.Multiply),
                new Unit("Inch", 39.37007874, Actions.Multiply),
                new Unit("Yard", 1.093613298, Actions.Multiply),
                new Unit("LightYear", 3.241e-17, Actions.Multiply),
                new Unit("Parsec", 3.241e-17, Actions.Multiply),
                new Unit("Miles", 0.000621371, Actions.Multiply),
                new Unit("Furlong", 0.00497097, Actions.Multiply)
            });
            Add("Flow", new Unit[]
            {
                new Unit("Liters/second",1, Actions.None),
                new Unit("Liters/minute" ,60 , Actions.Multiply),
                new Unit("Litres/hour" ,3600 , Actions.Multiply),
                new Unit("Gallons/hour" ,951.0193885 , Actions.Multiply),
                new Unit("Gallons/minute",15.85032314 , Actions.Multiply),
                new Unit("Gallons/second" ,0.264172052 , Actions.Multiply),
                new Unit("Meters3/hour" ,39878 , Actions.Multiply),
                new Unit("Meters3/minute", 0.06 , Actions.Multiply),
                new Unit("Meters3/second", 0.001 , Actions.Multiply)
            });
            Add("Area", new Unit[]
            {
                new Unit("Meters2" , 1, Actions.None),
                new Unit("Acres" , 0.000247104, Actions.Multiply),
                new Unit("Feet2" , 10.76391042, Actions.Multiply),
                new Unit("Hectares" , 0.0001, Actions.Multiply),
                new Unit("Inches2" , 1550.0031, Actions.Multiply),
                new Unit("Miles2" , 3.86E-07 , Actions.Multiply),
                new Unit("Yards2" , 1.195990046, Actions.Multiply)
            });
            Add("Aceleration", new Unit[]
            {
                new Unit("Meters/sec2" , 1, Actions.None),
                new Unit("Feet/sec2" , 3.280839895, Actions.Multiply),
                new Unit("Gravity" , 0.101971621, Actions.Multiply),
                new Unit("Inches/sec2" , 39.37007874 , Actions.Multiply)
            });
            Add("Speed", new Unit[]
            {
                 new Unit("Meters/second" , 1, Actions.None),
                 new Unit("Meters/minute" , 60, Actions.Multiply),
                 new Unit("Meters/hour" , 3600, Actions.Multiply),
                 new Unit("Feet/second" , 3.280839895, Actions.Multiply),
                 new Unit("Feet/minute" , 196.8503937, Actions.Multiply),
                 new Unit("Feet/hour" , 11811.02362, Actions.Multiply),
                 new Unit("Knots" , 1.943844492, Actions.Multiply),
                 new Unit("Mach" , 0.003016955, Actions.Multiply),
                 new Unit("Miles/second" , 0.000621371, Actions.Multiply),
                 new Unit("Miles/minute" , 0.037282272, Actions.Multiply),
                 new Unit("Miles/hour" , 2.236936292, Actions.Multiply)
            });
            Add("Power", new Unit[]
            {
                new Unit("Watt" , 1, Actions.None),
                new Unit("Horsepower" , 0.001341022, Actions.Multiply)
            });
            Add("Pressure", new Unit[]
            {
                new Unit("Pascals" ,  1, Actions.None),
                new Unit("Atmospheres" , 9.87E-06, Actions.Multiply),
                new Unit("Bars" , 1.00E-05, Actions.Multiply),
                new Unit("Pounds/Foot2" , 0.020885434 , Actions.Multiply),
                new Unit("Pounds/Inch2" , 0.000145038, Actions.Multiply),
                new Unit("Tons/Foot2" , 1.04E-05 , Actions.Multiply),
                new Unit("Tons/Inch2" , 7.25E-08, Actions.Multiply),
                new Unit("Kilograms/Meter2" , 0.101971621, Actions.Multiply)
            });
            Add("Mass", new Unit[]
            {
                new Unit("Grams" , 1, Actions.None),
                new Unit("Carats" , 5, Actions.Multiply),
                new Unit("Grains" , 15.43235835, Actions.Multiply),
                new Unit("Ounces" , 0.035273962, Actions.Multiply),
                new Unit("Pennyweights" , 0.643014931, Actions.Multiply),
                new Unit("Pounds" , 0.002204623, Actions.Multiply),
                new Unit("Stones" , 0.000157473, Actions.Multiply),
                new Unit("Tons" , 1.00E-06, Actions.Multiply)
            });
            Add("Volume", new Unit[]
            {
                new Unit("Litres" , 1, Actions.None),
                new Unit("Inches3" , 61.02374409, Actions.Multiply),
                new Unit("Feet3" , 0.035314667, Actions.Multiply),
                new Unit("Yards3" ,  0.001307951, Actions.Multiply),
                new Unit("Cups" , 4.226752838, Actions.Multiply),
                new Unit("Gallons" , 0.219969152, Actions.Multiply),
                new Unit("Meters3" , 0.001, Actions.Multiply),
                new Unit("Ounces" , 35.19506424, Actions.Multiply),
                new Unit("Pints" , 2.113376419, Actions.Multiply),
                new Unit("Quarts" , 1.056688209, Actions.Multiply),
                new Unit("Tablespoons" , 67.6280454, Actions.Multiply),
                new Unit("Teaspoons" , 202.8841362, Actions.Multiply)
            });
            Add("Time", new Unit[]
            {
                new Unit("Second" , 1, Actions.None),
                new Unit("Minute" , 60 , Actions.Divide),
                new Unit("Hour" , 3600, Actions.Divide),
                new Unit("Day" , 86400, Actions.Divide),
                new Unit("Week", 604800, Actions.Divide),
                new Unit("Year", 31556925.9936, Actions.Divide),
                new Unit("Beats", 86.4, Actions.Divide)
            });
            Add("Temperature", new Unit[]
            {
                new Unit("Celsius", 1, Actions.None),
                new Unit("Kelvin", 273.15, Actions.Add),
                new Unit("Farenheit", 1.8, Actions.Multiply, 32)
            });
            Add("FileSize", new Unit[]
            {
                new Unit("Byte", 1, Actions.None),
                new Unit("KiloByte", 1024, Actions.Divide),
                new Unit("MegaByte", 1048576, Actions.Divide),
                new Unit("GigaByte", 1073741824, Actions.Divide),
                new Unit("TerraByte", 1099511627776, Actions.Divide),
                new Unit("PetaByte", 1125899906842624, Actions.Divide),
                new Unit("ExaByte", 1152921504606846976, Actions.Divide)
            });
        }
    }
}
