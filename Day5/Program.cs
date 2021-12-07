using System;
using System.Collections.Generic;
using Utility;

namespace Day5
{
    class Program
    {
        public struct segmentCoords // We create a structure to hold segment coordinates
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine("Exercise 1 : " + Exercise1(input));
            Console.WriteLine("Exercise 2 : " + Exercise2(input));
        }

        public static int Exercise1(List<string> input)
        {
            List<segmentCoords> segmentsList = new List<segmentCoords>(parseCoordinates(input));
            List<segmentCoords> trimmedSegmentsList = new List<segmentCoords>(removeDiagonals(segmentsList)); // we remove all the diagnals for this exercise
            int[,] lineMap = drawLines(trimmedSegmentsList);
            int overlappingPoints = countOverlappingPoints(lineMap);

            return overlappingPoints;

        }
        public static int Exercise2(List<string> input)
        {
            List<segmentCoords> segmentsList = new List<segmentCoords>(parseCoordinates(input));
            int[,] lineMap = drawLines(segmentsList);
            int overlappingPoints = countOverlappingPoints(lineMap);

            return overlappingPoints;

        }

        public static List<segmentCoords> parseCoordinates(List<string> segmentsCoordinates) // we create a list of all the segments and their coordinates
        {
            List<segmentCoords> segmentsCoordList = new List<segmentCoords>();
            foreach (string segment in segmentsCoordinates)
            {

                string cleanedSegment = segment.Replace(" -> ", ","); // Replacing the middle arrow in each line by a comma to split easily after
                string[] coordinates = cleanedSegment.Split(','); // Breaking each line into 4 coordinates
                segmentCoords buffer;
                buffer.x1 = int.Parse(coordinates[0]);
                buffer.y1 = int.Parse(coordinates[1]);
                buffer.x2 = int.Parse(coordinates[2]);
                buffer.y2 = int.Parse(coordinates[3]);
                segmentsCoordList.Add(buffer); // adding the newly created segment to the segments list
            }
            return segmentsCoordList;
        }

        public static int[,] drawLines(List<segmentCoords> mySegments)
        {
            int[,] lineMap = new int[1000, 1000]; // This is the maximum array size needed. Not optimal as 99% of it will probably be empty

            foreach (segmentCoords segment in mySegments)
            {
                int distancex = Math.Abs(segment.x2 - segment.x1); // Measure segment's horizontal length
                int distancey = Math.Abs(segment.y2 - segment.y1); // Measure segment's vertical length
                int startx, starty = 0; // Starting point to trace line. Only going from left to right
                int direction=0;
                // Looking for starting point
                if (segment.x1 < segment.x2) // Segment coordinates go from left to right
                {
                    startx = segment.x1;
                    starty = segment.y1;
                    // We want to trace the diagonal from left to right but we need to know if we have to go upwards or downwards
                    // Determining direction. Default is going flat (but not used in tat case, maybe later if I want to have a generic drawing method)
                    if (segment.y1 < segment.y2)
                        direction = 1;

                    else
                        direction = -1;
                }
                else if (segment.x1 > segment.x2) // Segment coordinates go from right to left
                {
                    startx = segment.x2;
                    starty = segment.y2;
                    // We want to trace the diagonal from left to right but we need to know if we have to go upwards or downwards
                    // Determining direction
                    if (segment.y1 > segment.y2)
                        direction = 1;

                    else
                        direction = -1;
                }
                else // we have a vertical segment
                { 
                    startx = segment.x1; // the startx is not important here, it won change
                    if (segment.y1 < segment.y2) // Checking to move upwards
                    {
                        starty = segment.y1;
                    }
                    else {
                         starty = segment.y2;
                    }
                }
                //
                // Line drawing
                //

                if (distancey == 0) // Horizontal segment
                {
                    for (int x = startx; x <= startx + distancex; x++)
                    {
                        lineMap[x, segment.y1]++;
                    }
                }
                else if (distancex == 0) // Vertical segment
                {
                    for (int y = starty; y <= starty + distancey; y++)
                    {
                        lineMap[startx, y]++;
                    }
                }
                else // Diagonal segment. if it reaches this point x and y will vary 
                {
                    int yOffset = 0;
                    for (int x = startx; x <= startx + distancex; x++)
                    {
                        lineMap[x, starty + (yOffset*direction)]++; // Direction being -1 or 1 it only allows to invert the sign of the y increment to go upwards or downwards
                        yOffset++; 
                    }
                }

            }
            return lineMap; 
        }

        public static List<segmentCoords> removeDiagonals(List<segmentCoords> segmentsList) // returns only the segments with x1=x2 or y1=y2
        {
            List<segmentCoords> trimmedSegmentsList = new List<segmentCoords>();
            foreach (segmentCoords segment in segmentsList)
            {
                if (segment.x1 == segment.x2 || segment.y1 == segment.y2)
                {
                    trimmedSegmentsList.Add(segment);
                }
            }
            return trimmedSegmentsList;
        }
        public static int countOverlappingPoints(int[,] lineMap) // goes through the whole table and returns the values >=2
        {
            int overlappingLinesPoints = 0;
            for (int i = 0; i < lineMap.GetLength(0); i++)
            {
                for (int j = 0; j < lineMap.GetLength(1); j++)
                {
                    if (lineMap[i, j] >= 2)
                    {
                        overlappingLinesPoints++;
                    }
                }
            }
            return overlappingLinesPoints;
        }
    }
}
