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
            Dictionary<string, List<string>> map = GenerateMap(input); // generate the map once for Ex 1 & 2
            Console.WriteLine(Exercise1(map));
            Console.WriteLine(Exercise2(map));
        }

        public static int Exercise1(Dictionary<string, List<string>> map) 
        {
            List<Stack<string>> pathsList = new List<Stack<string>>(); // Will store all the paths from start to end according to Part 1 rules
            List<string> visited = new List<string>(); // Will carry the list of visited caves along the graph traversal
            Stack<string> path = new Stack<string>(); // Will store the current path. Using a stack as we will use DFS algorithm
            visitCaves(ref pathsList, path, visited, "start", map); // We call the pathsList by ref to carry only 1 instance across all recursive calls of the function
            int answer = pathsList.Count;
            return answer;
        }

        public static int Exercise2(Dictionary<string, List<string>> map) 
        {
            List<Stack<string>> pathsList = new List<Stack<string>>();
            Dictionary<string, int> visited = new Dictionary<string, int>(); // We change the definition of the visited caves to keep the count of visits
            Stack<string> path = new Stack<string>();
            visitCavesTwice(ref pathsList, path, visited, "start", map);
            int answer = pathsList.Count;
            return answer;
        }

        public static void visitCaves(ref List<Stack<string>> pathsList, Stack<string> path, List<string> visited, string caveToVisit, Dictionary<string, List<string>> map)
        {
            // I won't comment too much, this is a classic recursive Depth-First Search graph traversal algorithm with a small adjustment to filter out big caves (they can appear multiple times in a path)
            
            path.Push(caveToVisit);
            if (caveToVisit == "end") // We reach the end of the graph. We add the current path to the list of valid paths
            {
                pathsList.Add(new Stack<string>(path));
                path.Pop();
                return;
            }
            if (caveToVisit == caveToVisit.ToLower()) // If a cave is a small cave, we need to mark it as visited
            {
                visited.Add(caveToVisit);
            }
            foreach (string nextCave in map[caveToVisit]) // We explore all the nodes connected to the current cave
            {
                if (!visited.Contains(nextCave)) // but only if they have not been visited before
                {
                    visitCaves(ref pathsList, path, visited, nextCave, map);
                }
            }
            path.Pop(); // Visit of this node is complete, we remove it from the current path
            visited.Remove(caveToVisit); // We unmark it from the visited caves as it might be visited in another path
            return;

        }


        public static void visitCavesTwice(ref List<Stack<string>> pathsList, Stack<string> path, Dictionary<string, int> visited, string caveToVisit, Dictionary<string, List<string>> map)
        {
            path.Push(caveToVisit);
            if (caveToVisit == "end") // We reach the end of the graph
            {
                pathsList.Add(new Stack<string>(path));
                path.Pop();
                return;
            }
            // Weŕe not at the end yet
            if (caveToVisit == caveToVisit.ToLower() && ((visited.ContainsValue(2) && !visited.ContainsKey(caveToVisit)) | !visited.ContainsValue(2))) // Checking that no small cave has been visited twice before or if that the case, we check that this one hasn't been visited before
            {
                if (visited.ContainsKey(caveToVisit)) // If this small cave has been visited already
                {
                    visited[caveToVisit]++; // we increase the visit counter
                }
                else
                {
                    visited.Add(caveToVisit, 1); // otherwise we add it to the list of visited caves
                }
            }
            
            foreach (string nextCave in map[caveToVisit]) // Sane as for ex 1, we visit every connected cave if it was not visited before or no small case has been visited twice yet
            {
                if (!visited.ContainsValue(2) | (visited.ContainsValue(2) && !visited.ContainsKey(nextCave))) // We go to the next node as long as there is no small cave visited twice or there is one but the next one hasn't been visited yet
                {
                    visitCavesTwice(ref pathsList, path, visited, nextCave, map);
                }
            }
            path.Pop(); // Visit of the children cave is over

            if (visited.ContainsKey(caveToVisit)) // If we were in a small cave
            {
                if (visited[caveToVisit] == 1) // if it had been visited once, we remove it from the visited ones
                    visited.Remove(caveToVisit);
                else if (visited[caveToVisit] == 2) // or if it had been visited twice we remove 1 iteration
                    visited[caveToVisit]--;
            }
            return;

        }
        public static Dictionary<string, List<string>> GenerateMap(List<string> caveLinks)
        {
            Dictionary<string, List<String>> map = new Dictionary<string, List<string>>();
            foreach (string line in caveLinks) // we process every caves connections
            {
                Tuple<string, string> link = parseLink(line);
                string cave1 = link.Item1;
                string cave2 = link.Item2;
                if (!map.ContainsKey(cave1)) // if we dont have the cave in the list yet, we add it
                {
                    map.Add(cave1, new List<string>());
                }
                if (!map.ContainsKey(cave2))
                {
                    map.Add(cave2, new List<string>());
                }

                // Due to part 2 we optimize the initial map to make start and end oriented edges oriented (end doesn't have children, start doesn't have parents)
                if (cave1 == "start")
                    map[cave1].Add(cave2);
                else if (cave1 == "end")
                    map[cave2].Add(cave1);
                else if (cave2 == "start")
                    map[cave2].Add(cave1);
                else if (cave2 == "end")
                    map[cave1].Add(cave2);
                else // no start or end is involved in the connection, we make it bi-directional
                {
                    map[cave1].Add(cave2);
                    map[cave2].Add(cave1);
                }

            }
            return map;
        }

        public static Tuple<string, string> parseLink(string caveLink) // Utility function to parse the input to generate a list of connections between cavs
        {
            string[] caves = caveLink.Split('-');
            Tuple<string, string> caveLinkTuple = new Tuple<string, string>(caves[0], caves[1]);
            return caveLinkTuple;
        }
    }

}
