using System;
using Utility;
using System.Collections.Generic;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine(Exercise1(input));
            //Console.WriteLine(Exercise2(input));
        }

        public static long Exercise1(List<string> input)
        {
            Dictionary<string, Cave> map = GenerateMap(input);
            long paths = visitCaves(map);
            return 0;
        }

        public static long visitCaves(Dictionary<string,Cave> map)
        {
            List<Stack<Cave>> paths = new List<Stack<Cave>>();
            foreach(KeyValuePair<string,Cave> current in map)
            {
                Stack<Cave> path = new Stack<Cave>();
                path.Push(map["start"]);
                

            }


            return 0;
        }

        public static Dictionary<string, Cave> GenerateMap(List<string> caveLinks)
        {
            Dictionary<string, Cave> map = new Dictionary<string, Cave>();
            foreach (string line in caveLinks)
            {
                Tuple<string, string> link = parseLink(line);
                Cave cave1 = new Cave(link.Item1);
                Cave cave2 = new Cave(link.Item1);
                if (!map.ContainsValue(cave1))
                {
                    map.Add(cave1.name,cave1);
                }
                if (!map.ContainsValue(cave2))
                {
                    map.Add(cave2.name,cave2);
                }
                cave1.ConnectCaves(cave2);
                cave2.ConnectCaves(cave1);
            }
            return map;
        }

        public static Tuple<string, string> parseLink(string caveLink)
        {
            string[] caves = caveLink.Split('-');
            Tuple<string, string> caveLinkTuple = new Tuple<string, string>(caves[0], caves[1]);
            return caveLinkTuple;
        }
    }
    public class Cave : IEquatable<Cave>
    {
        private List<Cave> neighbors;
        private bool visited;

        public string name { get; set; }


        public Cave(string caveName)
        {
            name = caveName;
        }

        public bool Equals(Cave b)
        {
            return (this.name == b.name);
        }

        public void ConnectCaves(Cave n)
        {

            if (!this.neighbors.Contains(n))
            {
                this.neighbors.Add(n);
                n.ConnectCaves(this);
            }

        }

        public bool IsVisited()
        {
            return visited;
        }
    }
}
