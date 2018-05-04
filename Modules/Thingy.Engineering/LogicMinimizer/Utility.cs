namespace Thingy.Engineering.LogicMinimizer
{
    internal static class Utility
    {
        ///<summary>
        /// Returns length balanced versions of a string.
        /// If given a=010 and b = 10101 it will return 00010 and 10101.
        /// </summary>
        internal static void GetBalanced(ref string a, ref string b)
        {
            while (a.Length != b.Length)
            {
                if (a.Length < b.Length)
                    a = "0" + a;
                else
                    b = "0" + b;
            }
        }

        /// <summary>
        /// Returns the number of binary differences when passed two integers as strings.
        /// </summary>
        internal static int GetDifferences(string a, string b)
        {
            GetBalanced(ref a, ref b);

            int differences = 0;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    differences++;

            return differences;
        }

        /// <summary>
        /// Retreives the number of '1' characters in a string.
        /// </summary>
        internal static int GetOneCount(string a)
        {
            int count = 0;

            foreach (char c in a)
                if (c == '1')
                    count++;

            return count;
        }

        /// <summary>
        /// Calculates a mask given two input strings.
        /// For example when passed a=1001 and b = 1101
        /// will return 1-01.
        /// </summary>
        internal static string GetMask(string a, string b)
        {
            GetBalanced(ref a, ref b);

            string final = string.Empty;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    final += '-';
                else
                    final += a[i];
            }

            return final;
        }
    }
}
