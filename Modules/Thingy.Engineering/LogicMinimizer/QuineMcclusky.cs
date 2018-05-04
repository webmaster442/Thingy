using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Engineering.LogicMinimizer
{
    public static class QuineMcclusky
    {
        /// <summary>
        /// Simplifies a givenset of implicants.
        /// </summary>
        private static bool Simplify(ref List<Implicant> implicants)
        {
            //Group by number of 1's and determine relationships by comparing.
            var groups = (from i in Group(implicants) orderby i.Key select i).ToDictionary(i => i.Key, i => i.Value);
            var relationships = new List<ImplicantRelationship>();
            for (int i = 0; i < groups.Keys.Count; i++)
            {
                if (i == (groups.Keys.Count - 1)) break;

                var thisGroup = groups[groups.Keys.ElementAt(i)];
                var nextGroup = groups[groups.Keys.ElementAt(i + 1)];

                var q = from a in thisGroup
                        from b in nextGroup
                        where Utility.GetDifferences(a.Mask, b.Mask) == 1
                        select new ImplicantRelationship(a, b);

                relationships.AddRange(q);
            }


            //For each relationship, find the affected minterms and remove them.
            //Then add a new implicant which simplifies the affected minterms.
            foreach (ImplicantRelationship relationship in relationships)
            {
                var removeList = new List<Implicant>();

                foreach (Implicant m in implicants)
                {
                    if (relationship.A.Equals(m) || relationship.B.Equals(m)) removeList.Add(m);
                }

                foreach (Implicant m in removeList) implicants.Remove(m);

                var newImplicant = new Implicant
                {
                    Mask = Utility.GetMask(relationship.A.Mask, relationship.B.Mask)
                };
                newImplicant.Minterms.AddRange(relationship.A.Minterms);
                newImplicant.Minterms.AddRange(relationship.B.Minterms);

                bool exist = false;
                foreach (Implicant m in implicants)
                {
                    if (m.Mask == newImplicant.Mask) exist = true;
                }

                if (!exist) //Why am I getting dupes?
                    implicants.Add(newImplicant);
            }

            //Return true if simplification occurred, false otherwise.
            return !(relationships.Count == 0);
        }

        /// <summary>
        /// Populates a matrix based on a given set of implicants and minterms.
        /// </summary>
        private static void PopulateMatrix(ref bool[,] matrix, List<Implicant> implicants, int[] inputs)
        {
            for (int m = 0; m < implicants.Count; m++)
            {
                int y = implicants.IndexOf(implicants[m]);

                foreach (int i in implicants[m].Minterms)
                    for (int index = 0; index < inputs.Length; index++)
                        if (i == inputs[index])
                            matrix[y, index] = true;
            }
        }

        /// <summary>
        /// Groups binary numbers based on 1's.
        /// Stores group in a hashtable associated with a list(bucket) for each group.
        /// </summary>
        private static Dictionary<int, List<Implicant>> Group(List<Implicant> implicants)
        {
            var group = new Dictionary<int, List<Implicant>>();
            foreach (Implicant m in implicants)
            {
                int count = Utility.GetOneCount(m.Mask);

                if (!group.ContainsKey(count))
                    group.Add(count, new List<Implicant>());

                group[count].Add(m);
            }

            return group;
        }

        /// <summary>
        /// Retreives the final simplified expression in readable format.
        /// </summary>
        private static string GetFinalExpression(List<Implicant> implicants, bool lsba = false, bool negate = false)
        {
            int longest = 0;
            string final = string.Empty;

            foreach (Implicant m in implicants)
                if (m.Mask.Length > longest)
                    longest = m.Mask.Length;

            for (int i = implicants.Count - 1; i >= 0; i--)
            {
                if (negate) final += implicants[i].ToVariableString(longest, lsba, negate) + " & ";
                else final += implicants[i].ToVariableString(longest, lsba, negate) + " + ";
            }

            string ret = (final.Length > 3 ? final.Substring(0, final.Length - 3) : final);
            switch (ret)
            {
                case " + ":
                    return "1";
                case "":
                    return "0";
                default:
                    return ret;
            }
        }

        private static bool ContainsSubList(List<int> list, List<int> OtherList)
        {
            bool ret = true;
            foreach (var item in OtherList)
            {
                if (!list.Contains(item))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }

        private static bool ContainsAtleastOne(List<int> list, List<int> OtherList)
        {
            bool ret = false;
            foreach (var item in OtherList)
            {
                if (list.Contains(item))
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Selects the smallest group of implicants which satisfy the equation from the matrix.
        /// </summary>
        private static List<Implicant> SelectImplicants(List<Implicant> implicants, int[] inputs)
        {
            var lstToRemove = new List<int>(inputs);
            var final = new List<Implicant>();
            int runnumber = 0;
            while (lstToRemove.Count != 0)
            {
                //Implicant[] weightedTerms = WeightImplicants(implicants, final, lstToRemove);
                foreach (var m in implicants)
                {
                    bool add = false;

                    if (ContainsSubList(lstToRemove, m.Minterms))
                    {
                        add = true;
                        if (lstToRemove.Count < m.Minterms.Count) break;
                    }
                    else add = false;

                    if (((lstToRemove.Count <= m.Minterms.Count) && add == false) || runnumber > 5)
                    {
                        if (ContainsAtleastOne(lstToRemove, m.Minterms) && runnumber > 5) add = true;
                    }

                    if (add)
                    {
                        final.Add(m);
                        foreach (int r in m.Minterms) lstToRemove.Remove(r);
                    }
                }
                foreach (var item in final) implicants.Remove(item); //ami benne van már 1x, az még 1x ne
                ++runnumber;
            }

            return final;
        }

        public static string GetSimplified(IEnumerable<LogicItem> List, int variables, bool hazardsafe = false, bool lsba = false, bool negate = false)
        {
            var implicants = new List<Implicant>();

            var items = (from i in List where i.Checked == true || i.Checked == null orderby i.Index ascending select i.Index).ToArray();
            var careminterms = (from i in List where i.Checked == true orderby i.Index ascending select i.Index).ToArray();



            foreach (var item in items)
            {
                var m = new Implicant();
                m.Mask = LogicItem.GetBinaryValue(item, variables);
                m.Minterms.Add(item);
                implicants.Add(m);
            }

            //int count = 0;
            while (Simplify(ref implicants))
            {
                //Populate a matrix.
                bool[,] matrix = new bool[implicants.Count, items.Length]; //x, y
                PopulateMatrix(ref matrix, implicants, items);
            }
            List<Implicant> selected;
            if (hazardsafe) selected = implicants;
            else selected = SelectImplicants(implicants, careminterms);
            return GetFinalExpression(selected, lsba, negate);
        }
    }
}
