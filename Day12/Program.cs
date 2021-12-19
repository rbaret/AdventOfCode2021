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
            Dictionary<string, List<string>> map = GenerateMap(input);
            Console.WriteLine(Exercise1(map));
            //Console.WriteLine(Exercise2(input));
        }

        public static int Exercise1(Dictionary<string, List<string>> map)
        {
            List<Stack<string>> pathsList = new List<Stack<string>>();
            List<string> visited = new List<string>();
            Stack<string> path = new Stack<string>();
            visitCaves(ref pathsList, path, visited, "start", map);
            int answer = pathsList.Count;
            return answer;
        }

        public static int Exercise2(Dictionary<string, List<string>> map)
        {
            List<Stack<string>> pathsList = new List<Stack<string>>();
            List<string> visited = new List<string>();
            Stack<string> path = new Stack<string>();
            visitCaves(ref pathsList, path, visited, "start", map);
            int answer = pathsList.Count;
            return answer;
        }

        public static void visitCaves(ref List<Stack<string>> pathsList, Stack<string> path, List<string> visited, string caveToVisit, Dictionary<string, List<string>> map)
        {
            path.Push(caveToVisit);
            if (caveToVisit == "end")
            {
                pathsList.Add(new Stack<string>(path));
                path.Pop();
                return;
            }
            if (caveToVisit == caveToVisit.ToLower())
            {
                visited.Add(caveToVisit);
            }
            foreach (string nextCave in map[caveToVisit])
            {
                if (!visited.Contains(nextCave))
                {
                    visitCaves(ref pathsList, path, visited, nextCave, map);
                }
            }
            path.Pop();
            visited.Remove(caveToVisit);
            return;

        }

        public static Dictionary<string, List<string>> GenerateMap(List<string> caveLinks)
        {
            Dictionary<string, List<String>> map = new Dictionary<string, List<string>>();
            foreach (string line in caveLinks)
            {
                Tuple<string, string> link = parseLink(line);
                string cave1 = link.Item1;
                string cave2 = link.Item2;
                if (!map.ContainsKey(cave1))
                {
                    map.Add(cave1, new List<string>());
                }
                if (!map.ContainsKey(cave2))
                {
                    map.Add(cave2, new List<string>());
                }
                map[cave1].Add(cave2);
                map[cave2].Add(cave1);
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

}
