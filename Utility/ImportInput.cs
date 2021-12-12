using System;
using System.IO;
using System.Collections.Generic;
namespace Utility
{
    public static class ImportInput
    {
        public static List<int> ToIntList(string @path){
            List<int> myList = new List<int>();
            foreach(string line in File.ReadLines(@path)){
                myList.Add(int.Parse(line));
            }
            return myList;
        }
        public static List<string> ToStringList(string @path){
            List<string> myList = new List<string>();
            foreach(string line in File.ReadAllLines(@path)){
                myList.Add(line);
            }
            return myList;
        }

        public static char[][] ToCharArray(string @path){
            string[] fileContent = File.ReadAllLines(@path);
            char[][] myArray = new char[fileContent.Length][];
            int lineIndex = 0;
            foreach(string line in fileContent){
                myArray[lineIndex] = line.ToCharArray();
                lineIndex++;
            }
            return myArray;
        }

        public static int[,] ToIntMatrix(string @path){
            string[] fileContent = File.ReadAllLines(@path);
            int[,] myArray = new int [fileContent.Length,fileContent[0].Length];
            int lineIndex = 0;
            foreach(string line in fileContent){
                int i=0;
                foreach(char c in line)
                {
                    myArray[lineIndex,i] = c-48; // Converts 
                    i++;
                }
                lineIndex++;
            }
            return myArray;
        }
    }
}
