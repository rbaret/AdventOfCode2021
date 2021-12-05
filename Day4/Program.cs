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
            string path = "./input.txt";
            List<string> values = Utility.ImportInput.ToStringList(path);
            Console.WriteLine("Exercise 1 : " + Exercise1(values));
            Console.WriteLine("Exercise 2 : " + Exercise2(values));
        }

        public static int Exercise1(List<string> values)
        {
            string drawnNumbersString = values.First(); // First line contains drawn numbers
            List<int> drawnNumbers = new List<int>();
            foreach (string number in drawnNumbersString.Split(','))
            {
                drawnNumbers.Add(int.Parse(number));
            }
            // We use a list of Dictionaries because this way we can use the number as key and a bool to store the mark status
            List<Dictionary<int, bool>> bingoCards = generateCards(values.Skip(2).ToList()); // Skip the first two lines to send only the cards content
            foreach (int drawnNumber in drawnNumbers) // Starting the drawing
            {
                foreach (Dictionary<int, bool> card in bingoCards) // Let's check each board
                {
                    bingoDrawNumber(card, drawnNumber); // The actual draw were we check and marke the drawn number
                    if (card.Count(kvp => kvp.Value.Equals(true)) >= 5) // We check for a bingo only if there is at lest 5 numbers mrked in the board
                    {
                        if (checkBingo(card)) // If the board is the one
                        {
                            return (calculateScore(card, drawnNumber)); // We return the result required
                        }
                    }
                }
            }
            return -1; // Not supposed to reach that point. Process error if itś the case
        }
        public static int Exercise2(List<string> values)
        {
            string drawnNumbersString = values.First();
            List<int> drawnNumbers = new List<int>();
            foreach (string number in drawnNumbersString.Split(','))
            {
                drawnNumbers.Add(int.Parse(number));
            }
            // We use a list of Dictionaries because this way we can use the number as key and a bool to store the mark status
            List<Dictionary<int, bool>> bingoCards = generateCards(values.Skip(2).ToList()); // Skip the first two lines to send only the cards content
            int cardCount = bingoCards.Count(); // We count the number of cards we have
            List<Dictionary<int, bool>> winningCards = new List<Dictionary<int, bool>>(); // We store all the boards which are winning
            foreach (int drawnNumber in drawnNumbers)
            {
                foreach (Dictionary<int, bool> card in bingoCards)
                {
                    bingoDrawNumber(card, drawnNumber);
                    if (card.Count(kvp => kvp.Value.Equals(true)) >= 5 && !winningCards.Contains(card)) // Check for bingo ONLY if at least 5 marked numbers AND the board was not winning before (to reduce the numbr of checks)
                    {
                        if (checkBingo(card)) // checking for bingo
                        {
                            winningCards.Add(card); // Adding the winning card to the list of winners to not process it again (apart from marking numbers. Could be optimized)
                            if (winningCards.Count==bingoCards.Count) // Once we reach the total number of cards with the winning one we finally have the last winning board !
                            {
                                return (calculateScore(card,drawnNumber)); // Same calculations as for ex1
                            }
                        }
                    }
                }
            }
            return -1; // Not supposed to reach this point unless processing error
        }

        public static List<Dictionary<int, bool>> generateCards(List<string> values)
        {
            List<Dictionary<int, bool>> cards = new List<Dictionary<int, bool>>();
            Dictionary<int, bool> buffer = new Dictionary<int, bool>();
            foreach (string row in values)
            {
                if (string.IsNullOrEmpty(row)) // Empty lines mark the separation between cards
                {
                    cards.Add(new Dictionary<int, bool>(buffer)); //Time to add the current card to the card list
                    buffer.Clear();
                }
                else
                {
                    List<string> rowTiles = row.Split(' ').ToList();
                    foreach (string tile in rowTiles)
                    {
                        if (tile != "")
                            buffer.Add(int.Parse(tile), false); // Was forced to move to int instead of string because of spaces in frnt of single digits :@
                    }
                }
            }
            return cards;
        }

        public static void bingoDrawNumber(Dictionary<int, bool> card, int number)
        {

            if (card.ContainsKey(number)) // We find the drawn number in the card so...
            {
                card[number] = true; // ...we mark it. Pretty obvious duuuh
            }
        }

        public static bool checkBingo(Dictionary<int, bool> card)
        {
            int markedNumbersCount = 0;
            // Check rows
            for (int i = 0; i < 25; i += 5)
            { // Going from 5 to 5 as all the values from input file have been inserted in reading order, every line starts every 5 elements
                for (int j = 0; j < 5; j++)
                { // Read all consecutive elements 5 by 5
                    if (card.ElementAt(i + j).Value == true)
                        markedNumbersCount++;
                }
                if (markedNumbersCount == 5)
                    return true; // OHhhh that's a Bingo !
                else
                    markedNumbersCount = 0; // Reset counter of marked numbers at the end of every new line if no bingo
            }

            for (int i = 0; i < 5; i++)
            { // Check columns. Going through the 5 items of the dictionary (start of columns, 0 to 4) 
                for (int j = 0; j < 25; j += 5)
                { // and from 5 to 5 onwards
                    if (card.ElementAt(i + j).Value == true)
                        markedNumbersCount++;
                }
                if (markedNumbersCount == 5)
                    return true; // Actually, it's just "Bingo" :)
                else
                    markedNumbersCount = 0; // Reset counter of marked numbers at the end of every column if no bingo
            }
            return false;
        }

        public static int calculateScore(Dictionary<int, bool> card, int drawnNumber)
        {
            int sum = 0;
            foreach (KeyValuePair<int, bool> number in card) // Go through each item in the card, no time to tinker with lambda expressions and shit :o
            {
                if (!number.Value) // the number is not marked ? Good...
                    sum += number.Key; //... add it to the sum
            }
            return sum * drawnNumber; // formula from the AoC instructions
        }


    }
}
