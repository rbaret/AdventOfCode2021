using System;
using System.Collections.Generic;
using System.IO;
using Utility;
namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine("Exercise 1 : " + Exercise1(input));
            Console.WriteLine("Exercise 2 : " + Exercise2(input));
        }

        public static int Exercise1(List<string> input)
        { // I leave the bruteforce way to show the thinking process. Obviously it scales like shit
            List<int> fishList = inputToList(input);
            List<int> newFishes = new List<int>();
            for (int i = 0; i < 80; i++)
            {
                foreach (int fish in fishList)
                {
                    if (fish == 0)
                    { // When a fish reaches the end of spaning cycle
                        newFishes.Add(6); // We reset its timer to 6
                        newFishes.Add(8); // We span a new fish with a timer to 8
                    }
                    else
                        newFishes.Add(fish - 1); // otherwise we just decrease its timer
                }
                fishList = new List<int>(newFishes); // OUH DIRTY DIRTY. POOR RAM
                newFishes = new List<int>();
            }
            return fishList.Count;
        }
        public static long Exercise2(List<string> input)
        {
            List<int> fishList = inputToList(input);
            long totalFishCount = spawnFishes(fishList, 256);
            return totalFishCount;
        }
        public static List<int> inputToList(List<string> input)
        { // Utility classe to break the input list into an array of int
            string[] rawFishesList = input[0].Split(',');
            List<int> fishes = new List<int>();
            foreach (string rawFish in rawFishesList)
            {
                fishes.Add(int.Parse(rawFish));
            }
            return (fishes);
        }
        public static long spawnFishes(List<int> fishList, int days) // Obviously splitting fishes by the number of days remaining beforer the next spawn and only adding numbers is easier...
        {

            long[] fishesByDaysLeft = new long[9]; // A table where the index is the fishes timer value from 0 to 8. And we only keep the count for each value
            long totalFishCount = 0;
            long newFishes = 0;
            for (int i = 0; i < 9; i++)
            {
                fishesByDaysLeft[i] = fishList.FindAll(e => e.Equals(i)).Count; // Initialization of the counters for startup fishes
            }
            for (int i = 0; i < days; i++)
            { // It's obviously smarter to segregate fishes based on their internal timers and only keep the count.
                newFishes = fishesByDaysLeft[0]; // The new fishes that will spawn correspond to the number of fishes with the timer set to 0
                for (int j = 0; j < 8; j++)
                { // All the other fishes see their timer decreased by moving them 1 index down in the table.
                    fishesByDaysLeft[j] = fishesByDaysLeft[j + 1];
                }
                fishesByDaysLeft[6] += newFishes; // Everytime we span a fish, its "parent" timer goes from 0 to 6, so we add all the new parents to the 6
                fishesByDaysLeft[8] = newFishes; // And all the new babies go to 8 (replacement not addition because all the previous 8 went to 7 and we did not reset the value)
            }
            foreach (long fishCount in fishesByDaysLeft)
            {
                totalFishCount += fishCount; // we only have to sum all the values in the table ez pz
            }

            return totalFishCount;
        }
    }
}
