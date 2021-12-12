using System;
using Utility;
using System.Threading;
namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            int[,] input = Utility.ImportInput.ToIntMatrix(path);
            //Console.WriteLine(Exercise1(input));
            Console.WriteLine(Exercise2(input));
        }

        public static long Exercise1(int[,] octopuses)
        {
            long flashes = 0;
            int[,] nextStep;
            bool[,] hasFlashed;
            for (int step = 1; step <= 100; step++)
            {
                Console.Clear();
                hasFlashed = new bool[octopuses.GetLength(0), octopuses.GetLength(1)];
                nextStep = computeNextStep(octopuses, hasFlashed);
                flashes += computeFlashes(ref nextStep);
                octopuses = nextStep.Clone() as int[,];
                displayMatrix(octopuses);
                Console.WriteLine("Step " + step);
                Thread.Sleep(200);

            }
            return flashes;
        }

        public static long Exercise2(int[,] octopuses)
        {
            long flashes = 0;
            int[,] nextStep;
            bool[,] hasFlashed;
            bool octoAreInSync = false;
            int step = 0;
            do
            {
                step++;
                Console.Clear();
                hasFlashed = new bool[octopuses.GetLength(0), octopuses.GetLength(1)];
                nextStep = computeNextStep(octopuses, hasFlashed);
                flashes += computeFlashes(ref nextStep);
                octopuses = nextStep.Clone() as int[,];
                displayMatrix(octopuses);
                Console.WriteLine("Step " + step);
                Thread.Sleep(50);
                octoAreInSync = checksync(octopuses);
            } while (!octoAreInSync);
            return step;
        }

        public static int[,] computeNextStep(int[,] octopuses, bool[,] hasFlashed)
        {
            for (int i = 0; i < octopuses.GetLength(0); i++)
            {
                for (int j = 0; j < octopuses.GetLength(1); j++)
                {
                    octopuses[i, j]++;
                    if (octopuses[i, j] > 9 && !hasFlashed[i, j])
                    {
                        flashNeighbors(ref octopuses, i, j, ref hasFlashed);
                    }
                }
            }
            return octopuses;
        }

        public static void flashNeighbors(ref int[,] octopuses, int x, int y, ref bool[,] hasFlashed)
        {

            int arrayHeight = octopuses.GetLength(0);
            int arrayWidth = octopuses.GetLength(1);
            if ((x >= 0) && (x < arrayHeight) && (y >= 0) && (y < arrayWidth))
                hasFlashed[x, y] = true;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (((x + i) >= 0) && ((x + i) < arrayHeight) && ((y + j) >= 0) && ((y + j) < arrayWidth) && !(i == 0 && j == 0)) // Ignoring out of bound indexes + the cell itself
                    {
                        octopuses[x + i, y + j]++;
                        if (octopuses[x + i, y + j] > 9 && !hasFlashed[x + i, y + j]) // If the neighbor value is >9 it means that it has already flashed once
                        {
                            flashNeighbors(ref octopuses, x + i, y + j, ref hasFlashed);
                        }
                    }
                }
            }

        }

        public static long computeFlashes(ref int[,] octopuses)
        {
            long flashes = 0;
            for (int i = 0; i < octopuses.GetLength(0); i++)
            {
                for (int j = 0; j < octopuses.GetLength(1); j++)
                {
                    if (octopuses[i, j] > 9)
                    {
                        octopuses[i, j] = 0;
                        flashes++;
                    }
                }
            }
            return flashes;
        }

        public static void displayMatrix(int[,] matrix)
        {
            Console.Clear();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static bool checksync(int[,] octopuses)
        {
            int val = octopuses[0, 0];
            for (int i = 0; i < octopuses.GetLength(0); i++)
            {
                for (int j = 0; j < octopuses.GetLength(1); j++)
                {
                    if (octopuses[i, j] != val)
                        return false;
                }
            }
            return true;
        }
    }
}
