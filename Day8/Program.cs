using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            List<string> input = Utility.ImportInput.ToStringList(path);
            Console.WriteLine("Exercise 1 : " + Exercise1(input));
            Console.WriteLine("Exercise 2 : " + Exercise2(input));
        }

        public static int Exercise1(List<string> input)
        {
            List<string[]> signals = new List<string[]>();
            int segments1478 = 0;
            int fieldLength = 0;
            foreach (string signal in input)
            {
                string[] fields = signal.Split(' ');
                for (int i = 11; i <= 14; i++) // Only use the last 4 fields of each line (the ones after '|' )
                {
                    fieldLength = fields[i].Length; // Measure each field
                    if (fieldLength == 2 | fieldLength == 3 | fieldLength == 4 | fieldLength == 7) // and detect values 1,4,7 and 8 based on lengths (2,3,4,7)
                        segments1478++;
                }
            }
            return segments1478;
        }
        public static int Exercise2(List<string> input)
        {
            List<string[]> signals = new List<string[]>(getSignals(input));
            return resolvePart2(signals);
        }

        public static List<string[]> getSignals(List<string> input)
        {
            List<string[]> signals = new List<string[]>();
            foreach (string signal in input)
            {
                string[] fields = signal.Split(' ');
                signals.Add(fields);
            }
            return signals;
        }

        public static int resolvePart2(List<string[]> signals)
        {
            Dictionary<int, Tuple<int, char[]>> values = new Dictionary<int, Tuple<int, char[]>>();
            int sum = 0;
            foreach (string[] signal in signals)
            {
                values = mapSignalsToValues(signal);
                int output = decodeSignal(signal, values);
                sum += output;
            }
            return sum;
        }

        public static int decodeSignal(string[] signal, Dictionary<int, Tuple<int, char[]>> values)
        {
            StringBuilder sb = new StringBuilder();
            string output;
            for (int i = 11; i <= 14; i++)
            {
                int signalValue = calculateSignalValue(signal[i].ToArray());
                int digit = values[signalValue].Item1;
                sb.Append(digit);
            }
            output = sb.ToString();
            return int.Parse(output);
        }
        public static Dictionary<int, Tuple<int, char[]>> mapSignalsToValues(string[] signals)
        {
            Dictionary<int, Tuple<int, char[]>> values = new Dictionary<int, Tuple<int, char[]>>();
            List<string> fiveSegments = new List<string>();
            List<string> sixSegments = new List<string>();
            int[] signalsValues = new int[10]; // To map digits/signals to the corresponding string value to search in dictionary based on digit
            for (int i = 0; i < 10; i++) // detect 1/4/7/8 based on the number of segments (chars) in each signal
            {
                char[] signalAsArray = signals[i].ToArray();
                int signalValue = calculateSignalValue(signalAsArray);
                switch (signals[i].Length)
                {
                    case 2:
                        values.Add(signalValue, new Tuple<int, char[]>(1, signalAsArray));
                        signalsValues[1] = signalValue;
                        break;
                    case 3:
                        values.Add(signalValue, new Tuple<int, char[]>(7, signalAsArray));
                        signalsValues[7] = signalValue;
                        break;
                    case 4:
                        values.Add(signalValue, new Tuple<int, char[]>(4, signalAsArray));
                        signalsValues[4] = signalValue;
                        break;
                    case 5:
                        fiveSegments.Add(signals[i]); // This is one of the 5-segment digits to guess
                        break;
                    case 6:
                        sixSegments.Add(signals[i]); // This is one of the 6-segment digits to guess
                        break;
                    case 7:
                        values.Add(signalValue, new Tuple<int, char[]>(8, signalAsArray));
                        signalsValues[8] = signalValue;
                        break;
                    case 0:
                        break;

                }

            }

            foreach (string signal in sixSegments) // Determine which string is which signal between 6/9/0. We need to ave run a first round to be sure to have the 4 basic digits
            {
                char[] signalAsArray = signal.ToArray();
                int signalValue = calculateSignalValue(signalAsArray);
                if (isContainedIn(values[signalsValues[4]].Item2, signal.ToArray()))
                {
                    values.Add(signalValue, new Tuple<int, char[]>(9, signalAsArray)); // the digit 4 fits entirely in 9 but not in 0/6
                    signalsValues[9] = signalValue;
                }
                else if (isContainedIn(values[signalsValues[7]].Item2, signal.ToArray()))
                {
                    values.Add(signalValue, new Tuple<int, char[]>(0, signalAsArray)); // 7 fits in 0 but not in 6/9
                    signalsValues[0] = signalValue;
                }
                else
                {
                    values.Add(signalValue, new Tuple<int, char[]>(6, signalAsArray)); // If it's neither 9 or 0 it's 6
                    signalsValues[6] = signalValue;
                }
            }
            foreach (string signal in fiveSegments) // Determine which string is which digit between 2/3/5. This can only happen after we found 1 and 6
            {
                char[] signalAsArray = signal.ToArray();
                int signalValue = calculateSignalValue(signalAsArray);
                if (isContainedIn(values[signalsValues[1]].Item2,signal.ToArray()))
                {
                    values.Add(signalValue, new Tuple<int, char[]>(3, signalAsArray)); // 1 fits in 3 but not in 2/5
                    signalsValues[3] = signalValue;
                }
                else if (isContainedIn(signal.ToArray(), values[signalsValues[6]].Item2))
                {
                    values.Add(signalValue, new Tuple<int, char[]>(5, signalAsArray)); // 5 fits in 6 but 2 doesn't
                    signalsValues[5] = signalValue;
                }
                else
                {
                    values.Add(signalValue, new Tuple<int, char[]>(2, signalAsArray)); // not 3 or 5 ? then it's 2
                    signalsValues[2] = signalValue;
                }
            }
            return values;
        }
        public static int calculateSignalValue(char[] signal) // We calculate the value of the string coding a signal (based on the sum of ascii char values)
        {
            int sum = 0;
            foreach (char c in signal)
            {
                sum += c*c; // I use the square because as all chars being close to each other (a->f go from 97 to 102), I hit some values collisions between 2 signals
            }
            return sum;
        }

        public static bool isContainedIn(char[] signalToTest, char[] containsSignal) // Testing if a digit is contined fully into another (7 in 0, 4 in 9, 1 in 3, 5 in 6)
        {
            bool isContainedIn = true;
            foreach (char c in signalToTest)
            {
                if (!containsSignal.Contains(c))
                    isContainedIn = false;
            }
            return isContainedIn;
        }
    }
}