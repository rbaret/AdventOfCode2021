using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".\\input.txt";
            List<string> values = Utility.ImportInput.ToStringList(path);
            int Exercise1 = Bingo(values);
        }

        public static int Bingo(List<string> values)
        {
            string drawnValues = values.First();
            // We use a list of Dictionaries because this way we can use the number as key and a bool to store the mark status
            List<Dictionary<int, bool>> bingoCards = generateCards(values.Skip(2).ToList()); // Skip the first two lines to send only the cards content
            Console.WriteLine(drawnValues);
            return 1;
        }

        public static List<Dictionary<int, bool>> generateCards(List<string> values)
        {
            List<Dictionary<string, bool>> cards = new List<Dictionary<string, bool>>();
            Dictionary<string, bool> buffer = new Dictionary<string, bool>();
            foreach (string row in values)
            {
                List<string> rowTiles = row.Split(' ').ToList;
                foreach (string tile in rowTiles) { }
                if (string.IsNullOrWhiteSpace)
                {
                    cards.Add(new Dictionary<int, bool>(buffer.ToDictionary));
                    buffer.Clear();
                    rowNum = 0;
                    colNum = 0;
                }
            }
            return cards;
        }
    }
}
