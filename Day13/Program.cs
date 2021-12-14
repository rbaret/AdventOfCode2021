using System;
using Utility;
using System.Linq;
using System.Collections.Generic;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine(Exercise1(input));
            Exercise2(input);
        }

        /// <summary>Solves Exercise1 : fold the instructions once</summary>
        /// <returns>The answer to the exercise as an int</summary>
        public static int Exercise1(List<string> input)
        {
            List<Tuple<int, int>> dotsList = new List<Tuple<int, int>>();
            List<Tuple<string, int>> foldingInstructions = new List<Tuple<string, int>>();
            generateDotsList(ref dotsList, ref foldingInstructions, input);
            foldOnce(ref dotsList, foldingInstructions[0]);
            int visibleDots = dotsList.Count;
            return visibleDots;
        }

        /// <summary>Solves Exercise2 : fold the paper along all instructions</summary>
        public static void Exercise2(List<string> input)
        {
            List<Tuple<int, int>> dotsList = new List<Tuple<int, int>>();
            List<Tuple<string, int>> foldingInstructions = new List<Tuple<string, int>>();
            generateDotsList(ref dotsList, ref foldingInstructions, input);
            foreach (Tuple<string, int> instruction in foldingInstructions)
            {
                foldOnce(ref dotsList, instruction);
            }
            dotsList.TrimExcess(); // Let's reduce the list capacity
            displayDots(dotsList);
        }

        /// <summary>Fills the dots coordinates list as well as the folding instructions list (axis, and value)</summary>
        public static void generateDotsList(ref List<Tuple<int, int>> dotsList, ref List<Tuple<string, int>> foldingInstructions, List<string> input)
        {
            string[] coords;
            string[] instructions;
            int lineNr = 0;

            // Read only dots coordinates
            do
            {
                coords = input[lineNr].Split(',');
                dotsList.Add(new Tuple<int, int>(int.Parse(coords[0]), int.Parse(coords[1])));
                lineNr++;
            } while (!string.IsNullOrEmpty(input[lineNr]));

            lineNr++; // Skip the blank line

            // Read folding instructions starting the next line
            do
            {
                instructions = input[lineNr].Substring(input[lineNr].LastIndexOf(' ') + 1).Split('='); // We split the end of the line after the last space
                foldingInstructions.Add(new Tuple<string, int>(instructions[0], int.Parse(instructions[1]))); // We convert the string to int for the position
                lineNr++;
            } while (lineNr < input.Count); // Stp at the end of the list

        }

        /// <summary>Performs the folding of the given List of dots along the give axis direction and position</summary>
        public static void foldOnce(ref List<Tuple<int, int>> dotsList, Tuple<string, int> instruction)
        {
            string foldingAxis = instruction.Item1;
            int axisPosition = instruction.Item2;
            int distanceToAxis = 0;
            int targetXPos = 0;
            int targetYPos = 0;
            Tuple<int, int> targetDot;
            List<Tuple<int, int>> dotsToFold = new List<Tuple<int, int>>();

            // Fold the instructions
            // Along x axis
            if (foldingAxis == "x")
            {
                dotsToFold = dotsList.FindAll(d => d.Item1 > axisPosition); // We will move all the dots at the right of the folding axis, meaning all dots with an x value over the one in the instruction
            }
            else // Fold along y axis
            {
                dotsToFold = dotsList.FindAll(d => d.Item2 > axisPosition);
            }
            foreach (Tuple<int, int> dot in dotsToFold)
            { // We work on the small subset of dots to move only

                // The actual folding algorithm
                // To get the position of the target do we calculate the distance between the dot and the axis and remove it from the axis value
                if (foldingAxis == "x")
                {
                    distanceToAxis = dot.Item1 - axisPosition; // We calculate the distance between the dot and te axis
                    targetXPos = axisPosition - distanceToAxis; // and we create/modify the dot this distance away from the folding axis in the opposite direction
                    targetYPos = dot.Item2; // If we fold along x axis, y doesn't change
                }
                else
                {   // same as above but x doesn't change 
                    distanceToAxis = dot.Item2 - axisPosition;
                    targetXPos = dot.Item1;
                    targetYPos = axisPosition - distanceToAxis;
                }
                targetDot = new Tuple<int, int>(targetXPos, targetYPos);
                if (!dotsList.Contains(targetDot))// If the target doesn't contain a dot yet, we create it
                {
                    dotsList.Add(targetDot);
                    dotsList.Remove(dot);
                }
                // Otherwise, no need to do anything except deleting the origin dot as it's now in a different posistion
                else
                {
                    dotsList.Remove(dot);
                }
            }
        }

        ///<summary>Displays the final code by creating a small dot matrix</summary>
        public static void displayDots(List<Tuple<int, int>> dotsList)
        {
            
            char[][] matrix;
            int height = dotsList.Max(y=>y.Item2)+1; // We take the biggest y value in the dots list and transpose it to the 1st dimension size
            int width = dotsList.Max(y=>y.Item1)+1; // same with x and 2nd dimension of the array.
            matrix=new char[height][];
            for(int i=0;i<height;i++)
            {
                char[] line = new char[width];
                Array.Fill(line,' '); // Filling empty cells with spaces to display each column properly
                matrix[i] = line;
            }
            foreach(Tuple<int,int> dot in dotsList){
                matrix[dot.Item2][dot.Item1] = '#'; // Replace dots with # from the dots positions
            }
            for(int i = 0;i<height;i++)
            {
                Console.WriteLine(matrix[i]); // Display each line top to bottom to draw the text
            }
        }

    }
}