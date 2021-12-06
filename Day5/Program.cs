using System;
using System.Collections.Generic;
using Utility;

namespace Day5
{
    class Program
    {
        public struct segmentCoords
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
            //Console.WriteLine("Exercise 2 : " + Exercise2(input));
        }

        public static int Exercise1(List<string> input)
        {
            List<segmentCoords> segmentsList = new List<segmentCoords>(parseCoordinates(input));
            List<segmentCoords> trimmedSegmentsList = new List<segmentCoords>(removeDiagonals(segmentsList));
            int[,] lineMap = drawLines(trimmedSegmentsList);
            int overlappingPoints = countOverlappingPoints(lineMap);

            return overlappingPoints;

        }

        public static List<segmentCoords> parseCoordinates(List<string> segmentsCoordinates)
        {
            List<segmentCoords> segmentsCoordList = new List<segmentCoords>();
            foreach (string segment in segmentsCoordinates)
            {

                string cleanedSegment = segment.Replace(" -> ", ",");
                string[] coordinates = cleanedSegment.Split(',');
                segmentCoords buffer;
                buffer.x1 = int.Parse(coordinates[0]);
                buffer.y1 = int.Parse(coordinates[1]);
                buffer.x2 = int.Parse(coordinates[2]);
                buffer.y2 = int.Parse(coordinates[3]);
                segmentsCoordList.Add(buffer);
            }
            return segmentsCoordList;
        }

        public static int[,] drawLines(List<segmentCoords> mySegments)
        {
            int[,] lineMap = new int[1000, 1000];

            foreach (segmentCoords segment in mySegments)
            {
                int distancex = segment.x2 - segment.x1; // Measure segment's horizontal length
                int distancey = segment.y2 - segment.y1; // Measure segment's vertical length
                int startx, starty = 0; // Starting point to trace line. Only going from left to right, upwards
                startx = (distancex >= 0) ? segment.x1 : segment.x2; // Determining horizontal starting point
                starty = (distancey >= 0) ? segment.y1 : segment.y2; // Determining vertical starting point
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
                        lineMap[segment.x1, y]++;
                    }
                }
                else // Diagonal segment
                {
                    int yOffset = 0;
                    for (int x = startx; x <= startx + distancex; x++)
                    {
                        lineMap[x, starty + yOffset]++;
                        yOffset++;
                    }
                }

            }
            return lineMap; 
        }

        public static List<segmentCoords> removeDiagonals(List<segmentCoords> segmentsList)
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
        public static int countOverlappingPoints(int[,] lineMap)
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
