using System;
using Utility;
using System.Collections.Generic;
namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine(Exercise1(input));
            Console.WriteLine(Exercise2(input));
        }

        public static long Exercise1(List<string> input)
        {
            Stack<char> bracketsPile = new Stack<char>();
            List<char> leftBrackets = new List<char>() { '(', '{', '<', '[' };
            Dictionary<char, int> rightBrackets = new Dictionary<char, int>() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } }; // Bracket and associated score
            Dictionary<char, char> correspondance = new Dictionary<char, char>() { { '{', '}' }, { '(', ')' }, { '<', '>' }, { '[', ']' } }; // Bracket couples for easier search
            bool illegalFound = false;
            long syntaxScore = 0;
            foreach (string line in input)
            {
                int index = 0;
                bracketsPile.Clear(); // Start with an empty stack
                illegalFound = false;
                do
                {

                    char current = line[index];
                    if (bracketsPile.Count == 0) // The list is empty, only a left bracket is allowed
                    {
                        if (leftBrackets.Contains(current))
                        {
                            bracketsPile.Push(current);
                        }
                        else
                        {
                            illegalFound = true;
                            syntaxScore += rightBrackets[current];
                        }
                    }
                    else
                    { // The list is not empty, we can only add left brackets
                        if (leftBrackets.Contains(current)) // We have a left bracket
                            bracketsPile.Push(current); // W add it to the list
                        else // We have a right bracket
                        {
                            if (correspondance[bracketsPile.Peek()] == current) // This character is the corresponding char for the last item of the pile
                            {
                                bracketsPile.Pop();
                            }
                            else // ILLEGAL POLICE ALERT AAAAHHH
                            {
                                illegalFound = true;
                                syntaxScore += rightBrackets[current];
                            }
                        }
                    }
                    index++;
                } while (!illegalFound && index < line.Length);
            }
            return syntaxScore;

        }
        public static long Exercise2(List<string> input)
        {
            Stack<char> bracketsPile = new Stack<char>();
            List<char> leftBrackets = new List<char>() { '(', '{', '<', '[' }; // Used to check valid brackets to stack
            Dictionary<char, int> bracketScores = new Dictionary<char, int>() { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } }; // Bracket and associated score. We take the left bracket to remove 1 step in the matching of th value from expected closing bracket
            Dictionary<char, char> correspondance = new Dictionary<char, char>() { { '{', '}' }, { '(', ')' }, { '<', '>' }, { '[', ']' } }; // Bracket couples for easier search
            bool illegalFound = false;
            long currentScore = 0;
            List<long> scores = new List<long>();

            foreach (string line in input)
            {
                // Reset every value for each line
                int index = 0;
                bracketsPile.Clear();
                illegalFound = false;
                currentScore = 0;
                do
                {

                    char current = line[index];
                    if (bracketsPile.Count == 0) // The list is empty, only a left bracket is allowed
                    {
                        if (leftBrackets.Contains(current)) // Test if we have a left bracket
                        {
                            bracketsPile.Push(current); // If so, add it to the stack
                        }
                        else
                        {
                            illegalFound = true; // Otherwise mark the line as corrupted
                        }
                    }
                    else // The list is not empty, we can only add left brackets
                    {
                        if (leftBrackets.Contains(current)) // We have a left bracket
                            bracketsPile.Push(current); // We add it to the list
                        else // We have a right bracket
                        {
                            if (correspondance[bracketsPile.Peek()] == current) // If this character is the corresponding char for the last item of the pile
                            {
                                bracketsPile.Pop(); // We remove the closed bracket from the stack
                            }
                            else // ILLEGAL POLICE ALERT AAAAHHH
                            {
                                illegalFound = true; // Discard the line
                            }
                        }
                    }
                    index++;
                }
                while (!illegalFound && index < line.Length);

                if (!illegalFound && bracketsPile.Count > 0) // We have finished to read the line, time to check for icomplete lines
                {
                    do
                    {
                        currentScore = currentScore * 5 + bracketScores[bracketsPile.Pop()]; // score calculation based on the rules
                    } while (bracketsPile.Count > 0); // Do it until the stack is empty
                    scores.Add(currentScore); // Add the line score to the list
                }

            }

            // Final score calculation
            scores.Sort(); // Sort scores in the ascending order
            return scores[scores.Count / 2]; // Return the median score
        }
    }
}
