using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EduSpec.Code
{
    public class ReadTextFile
    {
        // Default folder  
        //static readonly string rootFolder = @"D:\Schools\Laerskool Kenmare\";
        //Default file  
        static readonly string textFile = @"D:\Schools\Laerskool Kenmare\Age 2.txt";

        public static void ReadText()
        {
            if (File.Exists(textFile))
            {
                // Read entire text file content in one string  
                string text = File.ReadAllText(textFile);
                //Console.WriteLine(text);
            }

            if (File.Exists(textFile))
            {
                // Read a text file line by line.  
                string[] lines = File.ReadAllLines(textFile);
                string Line;
                foreach (string line in lines)
                    Line = line;
                    //Console.WriteLine(line);
            }

            if (File.Exists(textFile))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(textFile))
                {
                    int counter = 0;
                    string ln;
                    string line;

                    while ((ln = file.ReadLine()) != null)
                    {
                        line = ln;
                        List<string> TagIds = line.Split(',').ToList();
                        counter++;
                    }
                    file.Close();
                    //Console.WriteLine($"File has {counter} lines.");
                }
            }

            //Console.ReadKey();

        }
    }
}
