using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".\\Day1.txt";
            Console.WriteLine(checkDepthIncreases(path));
        }
        private static int checkDepthIncreases(string path)
        {
            IEnumerable<string> lines = File.ReadAllLines(@path);
            int Increases = 0; // The first line will be an increase over the starting value
            int prevDepth = Int32.Parse(lines.First());
            foreach(string line in lines){
                int depth = Int32.Parse(line);
                Console.WriteLine(depth);
                if (depth>prevDepth)
                    Increases++;
                prevDepth = depth;
            }
            return Increases;
        }
    }
}