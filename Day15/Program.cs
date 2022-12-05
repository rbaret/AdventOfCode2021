using System.Linq;
using System;
using System.Collections.Generic;
using Utility;

namespace Day15
{
    public class Node
    {
        public int x;
        public int y;
        public int f = 0;
        public int g = 0;
        public int h = 0;
        public int risk;
        public Node parent;
        public Node(int x, int y, int risk)
        {
            this.x = x;
            this.y = y;
            this.risk = risk;
        }

        public Node(Node newNode, Node parent)
        {
            this.x = newNode.x;
            this.y = newNode.y;
            this.risk = newNode.risk;
            this.f = newNode.f;
            this.g = newNode.g;
            this.h = newNode.h;
            this.parent = parent;
        }

        public Node(Node n)
        {
            this.x = n.x;
            this.y = n.y;
            this.risk = n.risk;
            this.f = n.f;
            this.g = n.g;
            this.h = n.h;
            this.parent = n.parent;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Node n = (Node)obj;
            return (x == n.x && y == n.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = "input.txt";
            int[,] input = Utility.ImportInput.ToIntMatrix(path);
            Part1(input);
        }
        // Create a new type to represent a point in the cave

        // Part 1
        // Use A* to find the shortest path to the destination based on lowest risk probability
        // Starting point is (0,0), destination is input[intput.Length-1, input[0].Length-1]
        public static void Part1(int[,] input)
        {
            // Create a map of the cave with the risk value for each node

            Node[,] map = new Node[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    int risk = input[i, j];
                    map[i, j] = new Node(i, j, risk);
                }
            }

            Node start = map[0, 0];
            Node end = map[input.GetLength(0) - 1, input.GetLength(1) - 1];

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            // set g and f scores for start node
            start.g = start.f = start.h = 0;
            // add start to open list and strt algorithm
            openList.Add(start);

            // Crawl through the list of open nodes until we find the end node
            while (openList.Count > 0 && !closedList.Contains(end))
            {
                // find node with lowest f
                Node q = openList.Select(x => x).OrderBy(x => x.f).First();

                // Pop the node off the open list
                openList.Remove(q);
                if (q == end)
                {
                    break;
                }


                // for each neighbor of node
                List<Node> neighbors = GetNeighbors(q, map);
                foreach (Node neighbor in neighbors)
                {
                    // if neighbor is destination, stop search
                    if (neighbor == end)
                    {
                        break;
                    }
                    // else update neighbor's g and f
                    else
                    {
                        neighbor.g = q.g + neighbor.risk;
                        neighbor.h = Heuristic(neighbor.x, neighbor.y, end.x, end.y);
                        neighbor.f = neighbor.g + neighbor.h;
                    }

                    // if neighbor is in open list, but with higher g, skip
                    if (openList.Contains(neighbor))
                    {
                        if (openList.Find(x => x.x == neighbor.x && x.y == neighbor.y).g > neighbor.g)
                        {
                                continue;}
                    }

                    // else if neighbor is in closed list, but with lower g, skip
                    else if (closedList.Contains(neighbor))
                    {
                        Node n = new Node(closedList.Find(x => x.x == neighbor.x && x.y == neighbor.y));
                        if (n.g < neighbor.g)
                        {
                            continue;
                        }
                        else
                        {
                            closedList.Remove(n);
                            closedList.Add(neighbor);
                        }
                    }
                    // else add neighbor to open list
                    else
                    {
                        openList.Add(neighbor);
                    }
                }
                closedList.Add(q);
            }

            // Reconstruction of path
            Node pathNode = closedList.Last();
            int pathRisk = 0;
            while (pathNode.parent != null)
            {
                Console.WriteLine("Node: " + pathNode.x + ", " + pathNode.y + " Risk: " + pathNode.risk);
                pathRisk += pathNode.risk;
                pathNode = pathNode.parent;
            }

            Console.WriteLine("Part 1: The risk of the path to the exit is " + pathRisk);

        }

        // Heuristic calculated using manhattan distance
        public static int Heuristic(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static List<Node> GetNeighbors(Node n, Node[,] map)
        {

            // Return the nighbors for a given node
            // No diagonal neighbors, no out of bounds neighbors
            List<Node> neighbors = new List<Node>();
            if (n.x > 0)
            {
                neighbors.Add(new Node(map[n.x - 1, n.y], n));
            }
            if (n.x < map.GetLength(0) - 1)
            {
                neighbors.Add(new Node(map[n.x + 1, n.y], n));
            }
            if (n.y > 0)
            {
                neighbors.Add(new Node(map[n.x, n.y - 1], n));
            }
            if (n.y < map.GetLength(1) - 1)
            {
                neighbors.Add(new Node(map[n.x, n.y + 1], n));
            }
            return neighbors;
        }
    }
}
