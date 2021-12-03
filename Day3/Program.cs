using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".\\input.txt";
            List<string> values = Utility.ImportInput.ToStringList(path);
            Console.Write("Exercise 1 : "+Exercise1(values));
        }

        static int Exercise1(List<string> values)
        {
            int counter = 0;
            int[] bitWeight = Enumerable.Repeat(0, 12).ToArray(); // Array of 12 int set to 0 to store the sum of each bit position
            string gammaRate = "";
            string epsilonRate = "";
            StringBuilder gammaBuilder = new StringBuilder();
            StringBuilder epsilonBuilder = new StringBuilder();
            foreach (string value in values)
            {
                int index = 0;
                foreach (char bit in value)
                {
                    if (bit == '1')
                    {
                        bitWeight[index]++;
                    }
                    index++;
                }
                counter++; // Increment the number of lines read to know how many there are. Then used to detect most occuring bit
            }

            // Gamma/Epsilon rate calculation
            // Now let's divide each int by the total number of line. If the result is >0.5 then 1 is predominant, else 0 is
            foreach (int bitOccurences in bitWeight)
            {
                float predominance = (float)bitOccurences / (float)counter;
                if (predominance >= 0.5) // 1 is predominant for this bit index
                {
                    gammaBuilder.Append('1');
                    epsilonBuilder.Append('0');
                }
                else // 0 is predominant
                {
                    gammaBuilder.Append('0');
                    epsilonBuilder.Append('1');
                }
            }
            gammaRate = gammaBuilder.ToString();
            epsilonRate = epsilonBuilder.ToString();
            return (Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2));

        }
    }
}
