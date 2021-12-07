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
        public static List<int> ToIntList2(string @path){ // multiple ints per line, delimited by a char separator
            List<int> myList = new List<int>();
            string[] lines = File.ReadAllLines(@path);
            foreach(string line in lines){
                string[] lineNumbers = line.Split(',');
                foreach(string number in lineNumbers){
                    myList.Add(int.Parse(number));
                }
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
    }
}
