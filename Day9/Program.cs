using System;
using Utility;
using System.Collections.Generic;
using System.Diagnostics;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            Stopwatch myStopwatch = new Stopwatch();
            myStopwatch.Start();
            char[][] input = Utility.ImportInput.ToCharArray(path); // I should have used an int[][] bt didn want to waste time parsing
            Console.WriteLine("Data loading time : " + myStopwatch.Elapsed);
            myStopwatch.Restart();
            Console.WriteLine("Exercise 1 : " + Exercise1(ref input));
            myStopwatch.Stop();
            Console.WriteLine("Exercise 1 duration : " + myStopwatch.Elapsed);
            myStopwatch.Start();
            Console.WriteLine("Exercise 2 : " + Exercise2(ref input));
            myStopwatch.Stop();
            Console.WriteLine("Exercise 2 duration : " + myStopwatch.Elapsed);
        }

        public static int Exercise1(ref char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = findLocalLows(ref depthMap); // I decided to start immediately by keeping coordinates on tp of height, just in case
            int risk = 0;
            foreach (Tuple<int, int, int> localLow in localLows)
            {
                risk += localLow.Item3 - 48 + 1; // Calculation is made with ASCII value of the number because I was to lazy to int.parse() the chars, hence the -48
            }
            return risk;
        }

        public static long Exercise2(ref char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = findLocalLows(ref depthMap); // Tt was actually a good idea to keep the coordinates of the lower points
            List<Tuple<int, int>> basinTiles; // List used to track visited tiles
            List<int> allBasins = new List<int>(); // List which will store the size of all basins
            foreach (Tuple<int, int, int> localLow in localLows) // Let's check all lowest points and their basins
            {
                basinTiles = new List<Tuple<int, int>>(); // reinitialize the list for each basin as we don need to track coords from other basins (stated in the rules, 1 coord belong only to 1 basin)
                allBasins.Add(findBasin(ref depthMap, localLow, ref basinTiles)); // we pass the map ad the list of coordinates by ref (map is 10k entries, we better not pass it by copy). the list of tiles needs t be passed by ref to be completed at each recursive iteration
            }
            allBasins.Sort(); // Sort the final list
            allBasins.Reverse(); // And reverse it to sort it descending
            return allBasins[0] * allBasins[1] * allBasins[2]; // Return the product of the 3 biggest basin sizes
        }

        public static int findBasin(ref char[][] depthMap, Tuple<int, int, int> localLow, ref List<Tuple<int, int>> basinTiles)
        {
            int basinSize = 1; // the current tile is part of the basin
            int width = depthMap[0].Length; // we check the size of the map to avoid out of indexes
            int height = depthMap.Length; // same
            int x = localLow.Item1; // for easier writing/reading of the current coordinates
            int y = localLow.Item2;
            Tuple<int, int> coordinates = new Tuple<int, int>(x, y); // New tuple of coordinates to add to the basinś member tiles if not present already
            if (depthMap[x][y] == 57) // Value is 9 in ascii, this is not a basin tile
                return 0; // return 0 so it's not counted as a tile

            if (!basinTiles.Contains(coordinates)) basinTiles.Add(coordinates); // we have visited this tile, no need to check the neigbors !
            else return 0; // do not increase the size of the basin
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (((i == 0) ^ (j == 0)) && ((x + i) >= 0) && ((x + i) < height) && ((y + j) >= 0) && ((y + j) < width)) // test to exclude diagonal neighbors as well as out of bounds indexes (the firs is a XOR to easily avoid diagonals, we work onl when line OR column is the same as the current point)
                    {

                        if (depthMap[x + i][y + j] > depthMap[x][y]) // the neighbor is at higher altitude
                        {
                            if (!basinTiles.Contains(coordinates)) basinTiles.Add(coordinates); // let's add the current tile to the list
                            basinSize += findBasin(ref depthMap, new Tuple<int, int, int>(x + i, y + j, depthMap[x + i][y + j]), ref basinTiles); // and go visit the neighbor which will increase the basin size (or not if itś a 9)
                        }
                    }
                }
            }
            return basinSize; // finaly each visited tile which was not a 9 incremented the size of the basin by 1. Return the total
        }

        public static List<Tuple<int, int, int>> findLocalLows(ref char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = new List<Tuple<int, int, int>>(); // Storing the coordinates on top of the hsight will be useful later
            int arrayWidth = depthMap[0].Length - 1; // We take the size -1 because index starts at 0 and we want to avoid further -1 everywhere for tests
            int arrayHeight = depthMap.Length - 1; // same. 
            for (int i = 0; i <= arrayHeight; i++)
            {
                for (int j = 0; j <= arrayWidth; j++) // classic double loop
                {
                    int depth = depthMap[i][j];
                    int up, down, left, right;
                    up = down = left = right = depth + 1;
                    if (i - 1 >= 0) // we test for each direction if we're still in the table to avoid out of bound exceptions
                    {
                        up = depthMap[i - 1][j];
                    }
                    if (i + 1 <= arrayHeight)
                    {
                        down = depthMap[i + 1][j];
                    }
                    if (j - 1 >= 0)
                    {
                        left = depthMap[i][j - 1];
                    }
                    if (j + 1 <= arrayWidth)
                    {

                        right = depthMap[i][j + 1];
                    }
                    if (depth < up && depth < down && depth < left && depth < right)
                        localLows.Add(new Tuple<int, int, int>(i, j, depth));
                }
            }
            return localLows; // Final-fucking-ly !
        }
    }
}
