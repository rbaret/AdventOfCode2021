using System;
using System.Collections.Generic;
using Utility;
namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".\\input.txt";
            List<string> instructions = Utility.ImportInput.ToStringList(path);
            Console.WriteLine(Exercise1(instructions));
            Console.WriteLine(Exercise2(instructions));
        }

        static int Exercise1(List<string> instructions)
        {
            int depth = 0;
            int hPos = 0;
            foreach (string instruction in instructions)
            {
                string moveType = instruction.Substring(0, instruction.IndexOf(' '));
                int moveAmplitude = int.Parse(instruction.Substring(instruction.IndexOf(' ')));

                switch (moveType)
                {
                    case "up":
                        depth -= moveAmplitude;
                        break;
                    case "down":
                        depth += moveAmplitude;
                        break;
                    case "forward":
                        hPos += moveAmplitude;
                        break;
                }
            }
            return depth * hPos;

        }
        static int Exercise2(List<string> instructions)
        {
            int depth = 0;
            int hPos = 0;
            int aim = 0;
            foreach (string instruction in instructions)
            {
                string moveType = instruction.Substring(0, instruction.IndexOf(' '));
                int moveAmplitude = int.Parse(instruction.Substring(instruction.IndexOf(' ')));

                switch (moveType)
                {
                    case "up":
                        aim -= moveAmplitude;
                        break;
                    case "down":
                        aim += moveAmplitude;
                        break;
                    case "forward":
                        hPos += moveAmplitude;
                        depth += aim * moveAmplitude;
                        break;
                }
            }
            return depth * hPos;

        }
    }
}
