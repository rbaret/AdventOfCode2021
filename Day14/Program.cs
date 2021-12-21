using System;
using System.Text;
using Utility;
using System.Collections.Generic;
using System.Linq;
namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            string polymerTemplate = input[0];
            Dictionary<string, char> insertionRules = new Dictionary<string, char>();
            insertionRules = parseRules(input);
            Console.WriteLine("Exercise 1 : " + Exercise1(polymerTemplate, insertionRules));
            Console.WriteLine("Exercise 2 : " + Exercise2(polymerTemplate, insertionRules));
        }

        public static Dictionary<string, char> parseRules(List<string> input)
        {
            Dictionary<string, char> rules = new Dictionary<string, char>();
            input.RemoveRange(0, 2); // Remove the first two lines (template + blank line)
            foreach (string rule in input)
            {
                string[] assoc = rule.Split(" -> ");
                rules.Add(assoc[0], assoc[1][0]);
            }
            return rules;
        }

        public static long Exercise1(string polymerTemplate, Dictionary<string, char> rules)
        {
            StringBuilder sb;
            string polymer = new String(polymerTemplate);
            Dictionary<char, long> charOccurences = initCharOccurences(polymerTemplate);
            for (int step = 0;step < 10; step++)
            {
                sb = new StringBuilder();
                for (int i = 0; i < polymer.Length - 1; i++)
                {
                    char[] seq = new char[2];
                    seq[0] = polymer[i];
                    seq[1] = polymer[i + 1];
                    string seqStr = new string(seq);
                    if (rules.ContainsKey(seqStr))
                    {
                        char currentElement = rules[seqStr];
                        sb.Append(seq[0]);
                        sb.Append(currentElement);
                        if (charOccurences.ContainsKey(currentElement))
                        {
                            charOccurences[currentElement]++;
                        }
                        else
                        {
                            charOccurences.Add(currentElement, 1);
                        }
                    }
                }
                sb.Append(polymer[polymer.Length - 1]);
                polymer = sb.ToString();
            }
            return (charOccurences.Values.Max() - charOccurences.Values.Min());
        }

        public static long Exercise2(string polymerTemplate, Dictionary<string, char> rules)
        {
            string polymer = new String(polymerTemplate);
            Dictionary<string, long> seqOccurences = initSeqOccurences(polymerTemplate,rules); // Occurence of each sequeence in the polymer
            Dictionary<char, long> charOccurences = initCharOccurences(polymerTemplate); // Occurences of each char in the polymer
            for (int step = 0; step < 40; step++)
            {
                Dictionary<string, long> newSeqOccurences = new Dictionary<string, long>(seqOccurences);
                
                foreach (KeyValuePair<string, long> sequence in seqOccurences)
                {
                    if (sequence.Value>0)
                    {
                        char currentElement = rules[sequence.Key];
                        string newSeq1 = new string(new char[]{sequence.Key[0],currentElement});
                        string newSeq2 = new string(new char[]{currentElement,sequence.Key[1]});
                        newSeqOccurences[newSeq1]+=sequence.Value;
                        newSeqOccurences[newSeq2]+=sequence.Value;
                        charOccurences[currentElement]+=sequence.Value;
                        newSeqOccurences[sequence.Key]-=seqOccurences[sequence.Key];
                    }
                }
                seqOccurences = new Dictionary<string, long>(newSeqOccurences);
            }
            return (charOccurences.Values.Max() - charOccurences.Values.Min());
        }

        public static Dictionary<char, long> initCharOccurences(string polymerTemplate)
        {
            Dictionary<char, long> charOccurences = new Dictionary<char, long>();
            foreach (char c in polymerTemplate)
            {
                if (charOccurences.ContainsKey(c))
                    charOccurences[c]++;
                else
                {
                    charOccurences.Add(c, 1);
                }
            }
            return charOccurences;
        }

        public static Dictionary<string, long> initSeqOccurences(string polymerTemplate, Dictionary<string,char> rules)
        {
            Dictionary<string, long> seqOccurences = new Dictionary<string, long>();
            foreach(KeyValuePair<string,char> rule in rules)
            {
                seqOccurences.Add(rule.Key,0);
            }
            for (int i = 0; i < polymerTemplate.Length - 1; i++)
            {
                char[] seq = new char[2];
                seq[0] = polymerTemplate[i];
                seq[1] = polymerTemplate[i + 1];
                string seqStr = new string(seq);
                seqOccurences[seqStr]++;
            }
            return seqOccurences;
        }
    }
}
