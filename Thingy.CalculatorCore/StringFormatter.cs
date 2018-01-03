using AppLib.Maths;
using System;
using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Thingy.CalculatorCore
{
    public static class StringFormatter
    {
        public static string DisplayString(object o, bool preferPrefixes, bool groupByThousands, TrigonometryMode trigonometryMode)
        {
            Type t = o.GetType();
            switch (t.Name)
            {
                case "Double":
                case "Single":
                case "Int32":
                case "Int16":
                case "Byte":
                case "SByte":
                case "UInt32":
                case "UInt64":
                    return FormatDouble(Convert.ToDouble(o), preferPrefixes, groupByThousands);
                case "Complex":
                    return FormatComplex((Complex)o, trigonometryMode);
                case "String":
                    return (string)o;
                default:
                    if (o is IEnumerable) return FormatEnumerable(o, trigonometryMode);
                    else return o.ToString();
            }
        }

        private static string FormatDouble(double input, bool preferPrefixes, bool groupByThousands)
        {
            if (preferPrefixes)
            {
                var pfx = new PrefixDictionary();
                return pfx.DivideToPrefix(input);
            }
            if (groupByThousands)
            {
                string groupSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                string floatSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (double.IsNaN(input) || double.IsInfinity(input)) return input.ToString(CultureInfo.InvariantCulture);
                var sb = new StringBuilder();
                bool passed = false;
                int j = 1;
                int i;
                char[] ar;
                string text = input.ToString();
                if (text.Contains(floatSeparator))
                {
                    for (i = text.Length - 1; i >= 0; i--)
                    {
                        if (!passed && text[i] != floatSeparator[0]) sb.Append(text[i]);
                        else if (text[i] == floatSeparator[0])
                        {
                            sb.Append(text[i]);
                            passed = true;
                        }
                        if (passed && text[i] != floatSeparator[0])
                        {
                            sb.Append(text[i]);
                            if (j % 3 == 0) sb.Append(groupSeperator);
                            if (char.IsNumber(text[i])) j++;
                        }
                    }
                    ar = sb.ToString().ToCharArray();
                    Array.Reverse(ar);
                    return new string(ar).Trim();
                }
                else
                {
                    for (i = text.Length - 1; i >= 0; i--)
                    {
                        sb.Append(text[i]);
                        if (j % 3 == 0) sb.Append(groupSeperator);
                        if (char.IsNumber(text[i])) j++;
                    }
                    ar = sb.ToString().ToCharArray();
                    Array.Reverse(ar);
                    return new string(ar).Trim();
                }
            }
            else return input.ToString(CultureInfo.InvariantCulture);
        }

        private static string FormatComplex(Complex cplx, TrigonometryMode trigonometryMode)
        {
            var sb = new StringBuilder();
            sb.Append("R: ");
            sb.Append(cplx.Real);
            sb.Append(" i: ");
            sb.Append(cplx.Imaginary);
            sb.Append("\r\n φ: ");
            switch (trigonometryMode)
            {
                case TrigonometryMode.DEG:
                    sb.Append(Trigonometry.Rad2Deg(cplx.Phase));
                    sb.Append(" °");
                    break;
                case TrigonometryMode.GRAD:
                    sb.Append(Trigonometry.Rad2Grad(cplx.Phase));
                    sb.Append(" grad");
                    break;
                case TrigonometryMode.RAD:
                    sb.Append(cplx.Phase);
                    sb.Append(" rad");
                    break;
            }
            sb.Append(" ABS: ");
            sb.Append(cplx.Magnitude);
            return sb.ToString();
        }

        private static string FormatEnumerable(object o, TrigonometryMode trigonometryMode)
        {
            var sb = new StringBuilder();
            var coll = (IEnumerable)o;

            int idx = 0;
            if (o is Array || o is IList)
            {
                foreach (var i in coll)
                {
                    sb.AppendFormat("{0} => ", idx);
                    if (i is Complex) sb.Append(FormatComplex((Complex)i, trigonometryMode));
                    else sb.Append(i.ToString());
                    sb.Append("\n");
                    ++idx;
                }
            }
            else
            {
                foreach (var i in coll)
                {
                    if (i is Complex) sb.Append(FormatComplex((Complex)i, trigonometryMode));
                    else sb.Append(i.ToString());
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}
