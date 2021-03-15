using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DigitalDesignLib
{
    public static class FileWriter
    {
        public static void WriteFile(Dictionary<string, int> dict)
        {
            using (StreamWriter sw = new StreamWriter(@"..\..\..\Files\words.txt", false))
            {
                foreach (var item in dict)
                {
                    sw.WriteLine(item.Key + "_________" + item.Value);
                }
            }
        }
    }
}