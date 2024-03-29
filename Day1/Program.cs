﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility;
namespace AdventofCode
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".\\input.txt";
            List<int> values = Utility.ImportInput.ToIntList(path);
            Console.WriteLine("Exercise 1 : "+checkDepthIncreases(values));
            Console.WriteLine("Exercise 2 : "+CheckTripleDepthIncreases(values));
        }

        // Exercise 1
        private static int checkDepthIncreases(List<int> values)
        {
            int increases = 0;
            int prevDepth = values.First(); // We load the initial depth instead of 0, the depth might be either negative or positive, who knows ?
            foreach (int depth in values)
            {
                if (depth > prevDepth)
                    increases++;
                prevDepth = depth;
            }
            return increases;
        }

        // Exercise 2
        private static int CheckTripleDepthIncreases(List<int> values)
        {
            int[] valuesArray = values.ToArray();
            int increases = 0;
            int prevDepth = valuesArray[0] + valuesArray[1] + valuesArray[2];
            for (int i = 3; i < valuesArray.Length; i++)
            {
                int curDepth = prevDepth - valuesArray[i - 3] + valuesArray[i];
                if (curDepth > prevDepth)
                    increases++;
                prevDepth = curDepth;
            }

            return increases;
        }
    }
}