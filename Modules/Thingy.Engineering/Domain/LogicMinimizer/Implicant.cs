using System;
using System.Collections.Generic;
using System.Text;

namespace Thingy.Engineering.Domain.LogicMinimizer
{
    internal class Implicant
    {
        public string Mask { get; set; } //number mask.

        public List<int> Minterms { get; set; }

        public Implicant()
        {
            Minterms = new List<int>(); //original integers in group.
        }

        private const int ChrA = 65;

        public string ToVariableString(int length, bool lsba = false, bool negate = false)
        {
            StringBuilder strFinal = new StringBuilder();

            string mask = Mask;

            while (mask.Length != length)
            {
                mask = "0" + mask;
            }

            if (!lsba)
            {
                for (int i = 0; i < mask.Length; i++)
                {
                    if (negate)
                    {
                        if (mask[i] == '0')
                        {
                            strFinal.Append(Convert.ToChar(ChrA + i));
                            strFinal.Append("+");
                        }
                        else if (mask[i] == '1')
                        {
                            strFinal.Append(Convert.ToChar(ChrA + i));
                            strFinal.Append("'+");
                        }
                    }
                    else
                    {
                        if (mask[i] == '0')
                        {
                            strFinal.Append(Convert.ToChar(ChrA + i));
                            strFinal.Append("'");
                        }
                        else if (mask[i] == '1')
                        {
                            strFinal.Append(Convert.ToChar(ChrA + i));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < mask.Length; i++)
                {
                    if (negate)
                    {
                        if (mask[i] == '0')
                        {
                            strFinal.Append(Convert.ToChar((ChrA + (length - 1)) - i));
                            strFinal.Append("+");
                        }
                        else if (mask[i] == '1')
                        {
                            strFinal.Append(Convert.ToChar((ChrA + (length - 1)) - i));
                            strFinal.Append("'+");
                        }
                        if (i != mask.Length - 1)
                        {
                            strFinal.Append("+");
                        }
                    }
                    else
                    {
                        if (mask[i] == '0')
                        {
                            strFinal.Append(Convert.ToChar((ChrA + (length - 1)) - i));
                            strFinal.Append("'");
                        }
                        else if (mask[i] == '1')
                        {
                            strFinal.Append(Convert.ToChar((ChrA + (length - 1)) - i));
                        }
                    }
                }
            }
            if (negate)
            {
                return $"({strFinal.ToString().Remove(strFinal.Length - 1, 1)})";
            }

            return strFinal.ToString();
        }
    }
}
