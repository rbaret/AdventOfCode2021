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
        public int risk;
        public int cost = int.MaxValue;
        public Node parent = null;
        public Node(int x, int y, int risk)
        {
            this.x = x;
            this.y = y;
            this.risk = risk;
        }

        public Node(Node newNode, Node parent, int cost)
        {
            this.x = newNode.x;
            this.y = newNode.y;
            this.risk = newNode.risk;
            this.parent = parent;
            this.cost = cost;
        }

        public Node(Node n)
        {
            this.x = n.x;
            this.y = n.y;
            this.risk = n.risk;
            this.parent = n.parent;
            this.cost = n.cost;
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
            return HashCode.Combine(x, y);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = "input.txt";
            int[,] riskMap = Utility.ImportInput.ToIntMatrix(path);
            Part1(riskMap);
        }

        // Part 1
        // Use Dijkstra to find the shortest path to the destination based on lowest risk probability
        // Starting point is (0,0), destination is input[intput.Length-1, input[0].Length-1]
        public static void Part1(int[,] riskMap)
        {
            Node start = new Node(0, 0, riskMap[0, 0]);
            Node end = new Node(riskMap.GetLength(0) - 1, riskMap.GetLength(1) - 1, riskMap[riskMap.GetLength(0) - 1, riskMap.GetLength(1) - 1]);
            List<Node> path = Dijkstra(riskMap, start, end);
            displayPath(path);
            Console.WriteLine("Part 1: The path with the lowest risk to the exit has a risk of {0}", path.Select(x => x.risk).Sum());
        }

        public static List<Node> Dijkstra(int[,] riskMap, Node start, Node end)
        {
            // Create a map of the cave with the risk value for each node
            List<Node> path = new List<Node>();
            List<Node> visited = new List<Node>();
            List<Node> unvisited = new List<Node>();
            unvisited = initNodeList(riskMap);
            unvisited.Where(x => x.Equals(start)).First().cost = 0;
            while (unvisited.Count > 0 && !visited.Contains(end))
            {
                // Get the node with the lowest risk
                Node current = unvisited.OrderBy(x => x.cost).First();
                unvisited.Remove(current);
                visited.Add(current);
                List<Node> neighbors = GetValidNeighbors(riskMap, current);
                foreach (Node n in neighbors)
                {
                    if (!visited.Contains(n))
                    {
                        // If the node is already in the unvisited list, update the parent if the risk is lower
                        int newCost = current.cost + n.risk;
                        if(newCost < unvisited.Where(x => x.Equals(n)).First().cost)
                        {
                            unvisited.Where(x => x.Equals(n)).First().cost = newCost;
                            unvisited.Where(x => x.Equals(n)).First().parent = current;
                        }
                   }
                }
            }
            Node currentNode = visited.Last();
            // Reconstruct the path
            while (currentNode.parent != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        public static void displayPath(List<Node> path)
        {
            foreach (Node n in path)
            {
                Console.WriteLine("({0},{1}) Risk: {2}", n.x, n.y, n.risk);
            }
        }
        public static List<Node> initNodeList(int[,] riskMap)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < riskMap.GetLength(0); i++)
            {
                for (int j = 0; j < riskMap.GetLength(1); j++)
                {
                    nodes.Add(new Node(i, j, riskMap[i, j]));
                }
            }
            return nodes;
        }

        public static List<Node> GetValidNeighbors(int[,] riskMap, Node n)
        {

            // Return the nighbors for a given node
            // No diagonal neighbors, no out of bounds neighbors
            List<Node> neighbors = new List<Node>();
            if (n.x > 0)
            {
                neighbors.Add(new Node(n.x - 1, n.y, riskMap[n.x - 1, n.y]));
            }
            if (n.x < riskMap.GetLength(0) - 1)
            {
                neighbors.Add(new Node(n.x + 1, n.y, riskMap[n.x + 1, n.y]));
            }
            if (n.y > 0)
            {
                neighbors.Add(new Node(n.x, n.y - 1, riskMap[n.x, n.y - 1]));
            }
            if (n.y < riskMap.GetLength(1) - 1)
            {
                neighbors.Add(new Node(n.x, n.y + 1, riskMap[n.x, n.y + 1]));
            }
            return neighbors;
        }
    }
}
