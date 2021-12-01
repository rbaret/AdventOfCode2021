using System;
using System.Collections.Generic;
using System.IO;

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
            int Increases =0;
            int prevDepth =0;
            foreach(string line in lines){
                int depth = (int)line;
                if (depth>prevDepth)
                    Increases++;
            }
            return Increases;
        }
    }
}