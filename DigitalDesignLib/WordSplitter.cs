using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DigitalDesignLib
{
    public static class WordSplitter
    {
        static object locker = new object();
        private static Dictionary<string, int> Execute(string text)
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();
            Dictionary<string, int> wordsDict = new Dictionary<string, int>();
            SeparateWords(text, wordsDict);
            RemoveSpaces(wordsDict);
            Console.WriteLine(swatch.ElapsedMilliseconds);
            return SortDictionary(wordsDict);
        }
        private static void SeparateWords(string text, Dictionary<string, int> wordsDict)
        {
            lock (locker)
            {
                string[] words = text.Split(' ');
                foreach (var item in words)
                {
                    AddWord(ModifyWord(item), wordsDict);
                }
            }
        }
        private static void AddWord(string word, Dictionary<string, int> dict)
        {
            word = ModifyWord(word);
            Console.WriteLine(word);
            if (dict.ContainsKey(word))
            {
                dict[word]++;
            }
            else
            {
                dict.Add(word, 1);
            }
        }
        private static string ModifyWord(string word)
        {
            char[] CharsToTrim = { '.', ',', '!', '?', ':', '"', '-', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '{', '}', '(', ')' };
            while (word.Contains('<'))
            {
                int a = word.IndexOf('<');
                int b = word.IndexOf('>');
                word = word.Remove(a, b - a + 1);
            }
            word = word.Trim(CharsToTrim);
            return word.ToLower();
        }
        private static Dictionary<string, int> RemoveSpaces(Dictionary<string, int> dict)
        {
            foreach (var item in dict.ToArray())
            {
                if (item.Key == "" || item.Key == " " || item.Key == null || item.Key == " ")
                {
                    try
                    {
                        dict.Remove(item.Key);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
                }
            }
            return dict;
        }
        private static Dictionary<string, int> SortDictionary(Dictionary<string, int> dict)
        {

            dict = dict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return dict;

        }
        public static Dictionary<string, int> ExecuteMulty(string path)
        {
            List<string> wordsList = new List<string>();
            string line;
            Dictionary<string, int> wordsDict = new Dictionary<string, int>();
            Thread MyThread = new Thread(() => AddInDict(wordsList, wordsDict));
            MyThread.Start();
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("<p>") && line.EndsWith("</p>"))
                    {
                        line = line.Substring(3, line.Length - 7);
                        AddingCheckList(line, wordsList);
                    }
                }
            }
            Thread.Sleep(100);
            RemoveSpaces(wordsDict);
            //           Parallel.ForEach(wordsList, item => AddInDictParallel(item));
            return SortDictionary(wordsDict);
        }
        public static List<string> AddingCheckList(string line, List<string> wordsList)
        {
            lock (locker)
            {
                string[] words = line.Split(' ');
                foreach (var item in words)
                {
                    wordsList.Add(ModifyWord(item));
                }
            }
            return wordsList;
        }
        public static void AddInDict(List<string> list, Dictionary<string, int> dictionary)
        {
            Thread.Sleep(100);
            foreach (var item in list)
            {
                if (dictionary.ContainsKey(item))
                {
                    dictionary[item]++;
                }
                else
                {
                    dictionary.Add(item, 1);
                }
            }
        }
        public static void AddInDictParallel(string item)
        {
            try
            {
                //           Console.WriteLine(item);
                if (Parameter.dicts.ContainsKey(item))
                {
                    Parameter.dicts[item]++;
                }
                else
                {
                    Parameter.dicts.Add(item, 1);
                }
            }
            catch (NullReferenceException)
            {
                //         Console.WriteLine("SORRY");
            }
        }
        public static Dictionary<string, int> ExecuteParallel(string text)
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();
            Dictionary<string, int> wordsDict = new Dictionary<string, int>();
            SeparateWordsParallel(text, wordsDict);
            wordsDict=RemoveSpaces(wordsDict);
            Console.WriteLine(swatch.ElapsedMilliseconds);
            return SortDictionary(wordsDict);
        }
        public static void SeparateWordsParallel(string text, Dictionary<string, int> wordsDict)
        {
            string[] words = text.Split(' ');
            Parallel.ForEach(words, item => AddWordParallel(ModifyWord(item), wordsDict));
        }
        private static Dictionary<string, int> AddWordParallel(string word, Dictionary<string, int> dict)
        {
            word = ModifyWord(word);
            Console.WriteLine(word + "________Thread:" + Task.CurrentId);
            try
            {
                if (dict.ContainsKey(word) && word != null)
                {
                    dict[word]++;
                }
                else
                {
                    dict.Add(word, 1);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("SORRY");
            }
            return dict;
        }
    }
}