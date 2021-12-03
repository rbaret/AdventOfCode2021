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
            Console.WriteLine("Exercise 1 : " + Exercise1(values));
            Console.WriteLine("Exercise 2 : " + Exercise2(values));
        }

        static int Exercise1(List<string> values)
        {
            Tuple<string, string> rates = getGammaEpsilonRates(values);
            string gammaRate = rates.Item1;
            string epsilonRate = rates.Item2;
            return (Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2));

        }

        static int Exercise2(List<string> values)
        {
            Tuple<string, string> ratings = getOxygenCO2Scrubberratings(values);
            string oxGeneratorRating = ratings.Item1;
            string co2ScrubberRating = ratings.Item2;
            return (Convert.ToInt32(oxGeneratorRating, 2) * Convert.ToInt32(co2ScrubberRating, 2));

        }

        public static Tuple<string, string> getGammaEpsilonRates(List<string> values)
        {
            int counter = 0;
            int[] bitWeight = Enumerable.Repeat(0, 12).ToArray(); // Array of 12 int set to 0 to store the sum of each bit position
            string gammaRate = "";
            string epsilonRate = "";
            StringBuilder gammaBuilder = new StringBuilder();
            StringBuilder epsilonBuilder = new StringBuilder();
            Tuple<string, string> rates;
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
            // Now let's divide each int by the total number of lines. If the result is >0.5 then 1 is predominant, else 0 is (this is a simple average calculation)
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
            rates = new Tuple<string, string>(gammaRate, epsilonRate);
            return rates;
        }
        public static Tuple<string, string> getOxygenCO2Scrubberratings(List<string> values)
        {
            Tuple<string,string> gammaEpsilonRates; // We will reuse the resul of the function from exercise 1 to get the most prominent bit of the remaining items
            string gammaRate=""; 
            string epsilonRate="";
            List<string> currentValuesForOxy = new List<string>(values);
            List<string> currentValuesForCo2 = new List<string>(values);
            List<string> valuesToKeepForOxy = new List<string>();
            List<string> valuesToKeepForCo2 = new List<string>();
            string oxyRating = "";
            string co2Rating = "";
            Tuple<string, string> ratings;
            for (int bitIndex = 0; bitIndex < 12; bitIndex++)
            {
                gammaEpsilonRates = getGammaEpsilonRates(currentValuesForOxy); //Recalculating the most prominent bits of the remaining items in the list at each pass (suboptimal because we calculate for both rates and all bits but we don't care about perf here :o)
                gammaRate = gammaEpsilonRates.Item1;
                foreach (string value in currentValuesForOxy)
                {

                    if (value.ElementAt(bitIndex) == gammaRate.ElementAt(bitIndex))
                    {
                        valuesToKeepForOxy.Add(value);                        
                    }
                    
                }
                if (valuesToKeepForOxy.Count == 1)
                {
                    oxyRating = valuesToKeepForOxy.First();
                    break;
                }
                currentValuesForOxy = new List<string>(valuesToKeepForOxy);
                valuesToKeepForOxy = new List<string>();
            }

            for (int bitIndex = 0; bitIndex < 12; bitIndex++)
            {
                gammaEpsilonRates = getGammaEpsilonRates(currentValuesForCo2);
                epsilonRate = gammaEpsilonRates.Item2;
                foreach (string value in currentValuesForCo2)
                {
                    if (value.ElementAt(bitIndex) == epsilonRate.ElementAt(bitIndex))
                    {
                        valuesToKeepForCo2.Add(value);
                    }
                }

                
                if (valuesToKeepForCo2.Count == 1)
                {
                    co2Rating = valuesToKeepForCo2.First();
                    break;
                }
                currentValuesForCo2 = new List<string>(valuesToKeepForCo2);
                valuesToKeepForCo2 = new List<string>();


            }
            ratings = new Tuple<string, string>(oxyRating, co2Rating);
            return ratings;
        }
    }
}
