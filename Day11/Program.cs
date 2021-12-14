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
                hasFlashed = new bool[octopuses.GetLength(0), octopuses.GetLength(1)]; // Initializing the map of flash status
                nextStep = computeNextStep(octopuses, hasFlashed);// Define the status of each octopus for the next step
                flashes += computeFlashes(ref nextStep); // Make the ocot
                octopuses = nextStep.Clone() as int[,];
                displayMatrix(octopuses);
                Console.WriteLine("Step " + step);
                Thread.Sleep(200);

            }
            return flashes;
        }

        public static long Exercise2(int[,] octopuses) // The goal is to spot the moment where all the octopuses are flashing at the same moment
        {
            long flashes = 0;
            int[,] nextStep; // buffer array
            bool[,] hasFlashed; // Status map of all octopuses to know if they have already flashed or not. Passed by ref
            bool octoAreInSync = false; // The status we are checking every step
            int step = 0; // The count of steps to retun when all octopuses are in sync
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
                    octopuses[i, j]++; // increase the energy of each octopus by 1
                    if (octopuses[i, j] > 9 && !hasFlashed[i, j]) // If the octopus value was already 9 (or more) and it had not flashed already, make it flash
                    {
                        flashNeighbors(ref octopuses, i, j, ref hasFlashed); // Increase the value of all neighbors by 1. 
                    }
                }
            }
            return octopuses;
        }

        ///<summary>This function is increasing the value by 1 for all the neighbors of a flashing octopus
        /// If the neighbor octopus vale reaches or goes over 9, it calls itself with the coordinates of the neighbor which is trigerring a flash
        /// The hasFlashed status is set immediately as a neighbor would flash this octopus again by being triggered by the current flash and we don't want the current octopus to flash multiple times
        ///</summary>
        ///<param name="octopuses">You need to pass the octopuses map by ref to carry it through all the recursive calls</param>
        ///<param name="hasFlashed">You need to pass the octopuses flash status map by ref to carry it through all the recursive calls and prevent multiple flashing</param>

        public static void flashNeighbors(ref int[,] octopuses, int x, int y, ref bool[,] hasFlashed) // Flashing process : the current state of octopuses, the coordinates of the flash, the flash status of all octopuses (has flashed or not ?)
        {

            int arrayHeight = octopuses.GetLength(0);
            int arrayWidth = octopuses.GetLength(1);
            if ((x >= 0) && (x < arrayHeight) && (y >= 0) && (y < arrayWidth)) // If the coordinates of the flashing octopus are in the array, then this is a valid octopus
                hasFlashed[x, y] = true; // Then we change its state immediately to "has flashed" therefore the verification should not be needed has this function is supposed to be called only when an octopus is flashing because  we already know its value is 9 or more
            for (int i = -1; i <= 1; i++) // Starting to flash the neighbours. Classic double loop to flash a +/-1 zone around a given point 
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (((x + i) >= 0) && ((x + i) < arrayHeight) && ((y + j) >= 0) && ((y + j) < arrayWidth) && !(i == 0 && j == 0)) // Ignoring out of bound indexes + the cell itself
                    {
                        octopuses[x + i, y + j]++;
                        if (octopuses[x + i, y + j] > 9 && !hasFlashed[x + i, y + j]) // If the neighbor value is >9 it means that it might have already flashed once so we check its status in the hasFlahsed tabled
                        {
                            flashNeighbors(ref octopuses, x + i, y + j, ref hasFlashed); // And here is the damn recursion. As the function is not returning anything, this is why we pass the octopuses map by ref so we can carry it through various iterations
                        }
                    }
                }
            }

        }

        /// <summary> This function makes the calculation of the values of each octopus after the current step depending on their value. 
        /// We need to wait for the step to end before reseting flashing octopuses status to 0 otherwise they could start with a higher value instead
        /// although not flashing multiple times thanks to the hasFlashed status map
        /// </summary>
        /// <returns> The number of flashes for the current step</returns>

        public static long computeFlashes(ref int[,] octopuses)
        {
            long flashes = 0; /// Number of flashes for the current step
            for (int i = 0; i < octopuses.GetLength(0); i++)
            {
                for (int j = 0; j < octopuses.GetLength(1); j++)
                {
                    if (octopuses[i, j] > 9) // This value is over 9, the octopus has flashed
                    {
                        octopuses[i, j] = 0; // Reset status
                        flashes++; // Increase the number of flashes 
                    }
                }
            }
            return flashes; 
        }

        /// <summary>Displays the value of each octopus in a matrix, mainly for debuging reasons </summary>
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

        /// <summary>This is the function that checks the status of every octopus at the end of a step to check if all of them have flashed at the same time. Pure brute force</summary>
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
