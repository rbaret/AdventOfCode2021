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

        public static long Exercise1(string polymerTemplate, Dictionary<string, char> rules) // Dirty way, basically brute-forcing by generating the string every time
        {
            StringBuilder sb;
            string polymer = new String(polymerTemplate); // Not really needed. But we are working on a polymer, from the Template. We could just alter the template string
            Dictionary<char, long> charOccurences = initCharOccurences(polymerTemplate); // Initialize the occurences counter for each element of te polymer
            for (int step = 0;step < 10; step++) // 10 iterations as per the rules
            {
                sb = new StringBuilder(); // We will build the string every time. Yikes !
                for (int i = 0; i < polymer.Length - 1; i++) // Let's read the polymer elements 2 by 2, gotta stop at the one before the last.
                {
                    char[] seq = new char[2]; // We want to avoid using a stringbuiler for the small sequence, go for char array for mutability
                    seq[0] = polymer[i]; // First element of the current sequence
                    seq[1] = polymer[i + 1]; // Second element
                    string seqStr = new string(seq); // Convert the sequence to string for easier use in comparisons
                    if (rules.ContainsKey(seqStr)) // Actually not needed, the rules cover 100 cases for 10 chars which means all of the sequences are covered (10x10 chars)
                    {
                        char currentElement = rules[seqStr]; // get the element to add as per the rules
                        sb.Append(seq[0]); // Add the elements to the new polymer
                        sb.Append(currentElement);
                        if (charOccurences.ContainsKey(currentElement))
                        {
                            charOccurences[currentElement]++; // Increase the number of occurences of the added element
                        }
                        else // This if/else is not needed as all the chars are atually all present already in the starting sequence
                        {
                            charOccurences.Add(currentElement, 1);
                        }
                    }
                }
                sb.Append(polymer[polymer.Length - 1]); // Add the last element of the current polymer
                polymer = sb.ToString(); // Generat the new polymer string
            }
            return (charOccurences.Values.Max() - charOccurences.Values.Min()); // Thanks Linq for easing the pain of this part
        }

        public static long Exercise2(string polymerTemplate, Dictionary<string, char> rules)
        {
            string polymer = new String(polymerTemplate);
            Dictionary<string, long> seqOccurences = initSeqOccurences(polymerTemplate,rules); // Occurence of each sequence in the polymer
            Dictionary<char, long> charOccurences = initCharOccurences(polymerTemplate); // Occurences of each char in the polymer
            for (int step = 0; step < 40; step++) // 40 steps don't work AT ALL in stupid mode (generating the string every time)
            {
                Dictionary<string, long> newSeqOccurences = new Dictionary<string, long>(seqOccurences); // We need to keep track of sequences occurences instead like for the Lantern fish problem
                
                foreach (KeyValuePair<string, long> sequence in seqOccurences) // We go through the whole list of possible sequences (10 char, 100 possible sequences, all described in the rules)
                {
                    if (sequence.Value>0) // if the sequence is already appearing in the polymer
                    {
                        char currentElement = rules[sequence.Key]; // fetch the element to add as per the rules
                        string newSeq1 = new string(new char[]{sequence.Key[0],currentElement}); // Create a new sequence with the first element of the current sequence + new element 
                        string newSeq2 = new string(new char[]{currentElement,sequence.Key[1]}); // create a second sequence from the new element + second element of the current sequence
                        newSeqOccurences[newSeq1]+=sequence.Value; // Add as many occurences to these sequences as there are occurences of the current sequence 
                        newSeqOccurences[newSeq2]+=sequence.Value;
                        charOccurences[currentElement]+=sequence.Value; // Do the same for the occurences of the added element
                        newSeqOccurences[sequence.Key]-=seqOccurences[sequence.Key]; // AND HOLY FUCK DO NOT FORGET TO DESTROY ALL OCCURENCES OF THE CURRENT SEQUENCE WHICH DOESN'T EXIST ANYMORE BECAUSE IT WAS SPLIT IN 2 NEW DIFFERENT SEQS
                    } // yes you can feel my pain as why this fucking algorithm was supposed to be ok but took me so long to figure out why it was not
                }
                seqOccurences = new Dictionary<string, long>(newSeqOccurences); // we cannot modify the current occurences list during the loop so we have stored the new values in a buffer Dictionary which then becomes the new list of sequences occurences
            }
            return (charOccurences.Values.Max() - charOccurences.Values.Min());
        }

        public static Dictionary<char, long> initCharOccurences(string polymerTemplate) // Utility class to count each char at the start
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

        public static Dictionary<string, long> initSeqOccurences(string polymerTemplate, Dictionary<string,char> rules) // Same for 2-char sequences (we put all of them from the rules with a value of 0 occurences and increase with the starting polymer ones)
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
