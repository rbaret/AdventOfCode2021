using System;
using Utility;
using System.Collections.Generic;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            char[][] input = Utility.ImportInput.ToCharArray(path);
            Console.WriteLine("Exercise 1 : " + Exercise1(input));
            Console.WriteLine("Exercise 2 : " + Exercise2(input));
        }

        public static int Exercise1(char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = findLocalLows(depthMap);
            int risk = 0;
            foreach (Tuple<int, int, int> localLow in localLows)
            {
                risk += localLow.Item3 - 48 + 1; // Calculation is made with ASCII value of the number because I was to lazy to int.parse()
            }
            return risk;
        }

        public static long Exercise2(char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = findLocalLows(depthMap);
            List<Tuple<int,int>> bassinTiles;
            List<int> allBassins = new List<int>();
            foreach (Tuple<int, int, int> localLow in localLows)
            {
                bassinTiles = new List<Tuple<int, int>>();
                allBassins.Add(findBassin(depthMap, localLow,ref bassinTiles));
            }
            allBassins.Sort();
            allBassins.Reverse();
            return allBassins[0]*allBassins[1]*allBassins[2];
        }

        public static int findBassin(char[][] depthMap, Tuple<int, int, int> localLow,ref List<Tuple<int,int>> bassinTiles)
        {
            int bassinSize = 1;
            int width = depthMap[0].Length;
            int height = depthMap.Length;
            int x = localLow.Item1;
            int y = localLow.Item2;
            Tuple<int, int> coordinates = new Tuple<int, int>(x,y);
            if(depthMap[x][y]==57)
                return 0;

            if(!bassinTiles.Contains(coordinates)) bassinTiles.Add(coordinates);
            else return 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (((i == 0)^(j == 0)) && ((x + i) >= 0) && ((x + i) < height) && ((y + j) >= 0) && ((y + j) < width))
                    {
                        
                        if (depthMap[x+i][y+j] > depthMap[x][y])
                        { 
                            if(!bassinTiles.Contains(coordinates)) bassinTiles.Add(coordinates);
                            bassinSize += findBassin(depthMap,new Tuple<int, int, int>(x+i,y+j,depthMap[x+i][y+j]),ref bassinTiles); 
                        }
                    }
                }
            }
            return bassinSize;
        }

        public static List<Tuple<int, int, int>> findLocalLows(char[][] depthMap)
        {
            List<Tuple<int, int, int>> localLows = new List<Tuple<int, int, int>>();
            int arrayWidth = depthMap[0].Length - 1;
            int arrayHeight = depthMap.Length - 1;
            for (int i = 0; i <= arrayHeight; i++)
            {
                for (int j = 0; j <= arrayWidth; j++)
                {
                    int depth = depthMap[i][j];
                    int up, down, left, right = 0;
                    if (i > 0 && i < arrayHeight && j > 0 && j < arrayWidth) // middle cells
                    {
                        left = depthMap[i][j - 1];
                        right = depthMap[i][j + 1];
                        up = depthMap[i - 1][j];
                        down = depthMap[i + 1][j];
                        if (depth < left && depth < right && depth < up && depth < down)
                            localLows.Add(new Tuple<int, int, int>(i, j, depth));
                    }
                    else if (i == 0 && j > 0 && j < arrayWidth)
                    { // first row, no up
                        left = depthMap[i][j - 1];
                        right = depthMap[i][j + 1];
                        down = depthMap[i + 1][j];
                        if (depth < left && depth < right && depth < down)
                            localLows.Add(new Tuple<int, int, int>(i, j, depth));
                    }
                    else if (i > 0 && i < arrayHeight && j == 0) // first column, no left
                    {
                        right = depthMap[i][j + 1];
                        up = depthMap[i - 1][j];
                        down = depthMap[i + 1][j];
                        if (depth < up && depth < right && depth < down)
                            localLows.Add(new Tuple<int, int, int>(i, j, depth));
                    }
                    else if (i == arrayHeight && j > 0 && j < arrayWidth)
                    { // Last row, no down
                        left = depthMap[i][j - 1];
                        right = depthMap[i][j + 1];
                        up = depthMap[i - 1][j];
                        if (depth < left && depth < right && depth < up)
                            localLows.Add(new Tuple<int, int, int>(i, j, depth));
                    }
                    else if (i > 0 && i < arrayHeight && j == arrayWidth)
                    { // Last column, no right
                        left = depthMap[i][j - 1];
                        up = depthMap[i - 1][j];
                        down = depthMap[i + 1][j];
                        if (depth < left && depth < down && depth < up)
                            localLows.Add(new Tuple<int, int, int>(i, j, depth));

                    }

                }
            }
            if (depthMap[0][0] < depthMap[0][1] && depthMap[0][0] < depthMap[1][0]) // top left corner
                localLows.Add(new Tuple<int, int, int>(0, 0, depthMap[0][0]));

            if (depthMap[0][arrayWidth] < depthMap[0][arrayWidth - 1] && depthMap[0][arrayWidth] < depthMap[1][arrayWidth]) // top right corner
                localLows.Add(new Tuple<int, int, int>(0, arrayWidth, depthMap[0][arrayWidth]));

            if (depthMap[arrayHeight][0] < depthMap[arrayHeight - 1][0] && depthMap[arrayHeight][0] < depthMap[arrayHeight][1]) // bottom left corner
                localLows.Add(new Tuple<int, int, int>(arrayHeight, 0, depthMap[arrayHeight][0]));

            if (depthMap[arrayHeight][arrayWidth] < depthMap[arrayHeight][arrayWidth - 1] && depthMap[arrayHeight][arrayWidth] < depthMap[arrayHeight - 1][arrayWidth]) // bottom right corner
                localLows.Add(new Tuple<int, int, int>(arrayHeight, arrayWidth, depthMap[arrayHeight][arrayWidth]));

            return localLows;
        }
    }
}
