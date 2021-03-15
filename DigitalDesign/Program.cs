using System;
using System.IO;
using System.Collections.Generic;
using DigitalDesignLib;
using System.Reflection;
using System.Diagnostics;
using System.Threading;


namespace DigitalDesign
{
    class Program
    {
        public static string pathPrefix = @"..\..\Files\";
        //     private static Dictionary<string, int> ExecuteService(string text)
        private static Dictionary<string, int> ExecuteService(string text)
        {
            Console.WriteLine("sTART");
            var client = new DigitalDesignServiceReference.Service1Client();
            var q=client.GetDataUsingDataContract(new DigitalDesignServiceReference.CompositeType {Text= text, Dict=new Dictionary<string, int>() });
            //       var dick = client.Execute(text);
            //         return dick;

            Console.WriteLine(q.Text);
            var t=client.Execute(q.Text);
            foreach (var item in t)
            {
                Console.WriteLine(item);
            }
            return t;
             
        }
        static void Main(string[] args)
        {
            string line;
            string text = "";
            using (StreamReader sr = new StreamReader(pathPrefix + "book1.fb2"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("<p>") && line.EndsWith("</p>"))
                    {
                        line = line.Substring(3, line.Length - 7);
                        text += line;
                    }
                }
            }
            Console.WriteLine("Text is compiled");
            var dictionary = new Dictionary<string, int>();
            var t = typeof(WordSplitter);
            var f = t.GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Static);
    //        dictionary = (Dictionary<string, int>)f.Invoke(null, new object[] { text });
      //      dictionary = WordSplitter.ExecuteParallel(text);
            dictionary=ExecuteService(text);
            using (StreamWriter sw = new StreamWriter(@"..\..\Files\words.txt", false))
            {
                foreach (var item in dictionary)
                {
                    sw.WriteLine(item.Key + "____________________" + item.Value);
                }
            }
            foreach (var item in dictionary)
            {
                Console.WriteLine(item.Key + "__________" + item.Value);
            }
            Console.ReadLine();
        }

       
    }
}
