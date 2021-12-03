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
            foreach(string line in File.ReadLines(@path)){
                myList.Add(line);
            }
            return myList;
        }
    }
}
