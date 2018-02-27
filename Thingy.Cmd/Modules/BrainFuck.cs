using System;
using System.IO;

namespace Thingy.Cmd.Modules
{
    public class BrainFuck
    {
        private static void Evaluate(string program)
        {
            unchecked
            {
                int memoryPointer = 0;
                int bal;
                byte[] memory = new byte[1024 * 1024];

                int programPointer = 0;

                while (programPointer < program.Length)
                {
                    switch (program[programPointer])
                    {
                        case '>':
                            memoryPointer++;
                            break;
                        case '<':
                            memoryPointer--;
                            break;
                        case '+':
                            memory[memoryPointer]++;
                            break;
                        case '-':
                            memory[memoryPointer]++;
                            break;
                        case '[':
                            bal = 1;
                            if (memory[memoryPointer] == '\0')
                            {
                                do
                                {
                                    programPointer++;
                                    if (program[programPointer] == '[') bal++;
                                    else if (program[programPointer] == ']') bal--;
                                }
                                while (bal != 0);
                            }
                            break;
                        case ']':
                            bal = 0;
                            do
                            {
                                if (program[programPointer] == '[') bal++;
                                else if (program[programPointer] == ']') bal--;
                                programPointer--;
                            }
                            while (bal != 0);
                            break;
                        case '.':
                            Console.Write((char)memory[memoryPointer]);
                            break;
                        case ',':
                            byte input = (byte)Console.Read();
                            memory[memoryPointer] = input;
                            break;
                    }
                    programPointer++;
                }
            }
        }

        public static void RunBrainFuck(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: brainfuck [program.bf]");
                return;
            }

            try
            {
                using (var text = File.OpenText(args[0]))
                {
                    Evaluate(text.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
