namespace Thingy.Cmd
{
    internal static class ParameterParser
    {
        private static string[] Tokenize(string commandLine)
        {
            char[] parmChars = commandLine.ToCharArray();
            bool inQuote = false;
            for (int i = 0; i < parmChars.Length; i++)
            {
                if (parmChars[i] == '"')
                    inQuote = !inQuote;
                if (!inQuote && parmChars[i] == ' ')
                    parmChars[i] = '\n';
            }
            var argumentsArray = (new string(parmChars)).Split('\n');
            for (int i = 0; i < argumentsArray.Length; i++)
            {
                if (argumentsArray[i].StartsWith("\"") && argumentsArray[i].EndsWith("\""))
                {
                    argumentsArray[i] = argumentsArray[i].Substring(1, argumentsArray[i].Length - 2);
                }
            }
            return argumentsArray;
        }

        private static void Process(ref Parameters parameters, string[] tokens, out string program)
        {
            int i = 0;
            int increment = 0;
            program = null;
            do
            {
                var current = tokens[i];
                var next = GetNext(tokens, i);
                if (current.StartsWith("-"))
                {
                    if (!next.StartsWith("-") && !string.IsNullOrEmpty(next))
                    {
                        //switch with value
                        if (!parameters.SwithesWithValue.ContainsKey(current))
                            parameters.SwithesWithValue.Add(current, next);
                        else
                            parameters.SwithesWithValue[current] =  next;
                        increment = 2;
                    }
                    else
                    {
                        //single switch
                        parameters.Switches.Add(current);
                        increment = 1;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        //skip program name
                        increment = 1;
                        program = current;
                    }
                    else
                    {
                        //file
                        parameters.Files.Add(current);
                        increment = 1;
                    }
                }

                i += increment;
            }
            while (i < tokens.Length);
        }

        private static string GetNext(string[] commandLineParts, int i)
        {
            int nextindex = i + 1;
            if (nextindex < commandLineParts.Length) return commandLineParts[nextindex];
            else return "";
        }

        public static Parameters Parse(string commandline, out string commandName)
        {
            Parameters ret = new Parameters();
            string[] tokens = Tokenize(commandline);
            Process(ref ret, tokens, out commandName);
            return ret;
        }
    }
}
