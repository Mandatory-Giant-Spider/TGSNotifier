using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using TGSNotifier.Tagesschau;
using TGSNotifier.Tagesschau.Models;

namespace TagesschauScraper
{
    class Program
    {
        public static int Days { get; set; }
        public static void Main(String[] args)
        {
            // Create parser object and empty list for daily results
            var parser = new TagesschauParser(DateTime.Now.Date);
            var articleDataList = new List<ArticleData>();

            // Amount of days to parse. Default is 7!
            if (args.Length > 0 && int.TryParse(args[0], out int days))
            {
                Days = days;
            }
            else
            {
                Days = 7;
            }

            // Request the information for every single day
            for(int i = 0; i < Days; i++)
            {
                // Write date information to console
                Console.WriteLine($"Requesting data for {parser.Date.ToString("dd.MM.yyyy")}...");

                articleDataList.Add(parser.RequestData());
                parser.Date = parser.Date.AddDays(-1);

                // Wait for 3 seconds to avoid getting banned by Tagesschau
                Task.Delay(3000);
            }


            // Prepare write path
            string path = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\PoWi.txt";

            // Delete file if exists
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Open File "PoWi.txt" on Desktop and write the results into it
            FileStream file = File.OpenWrite(path);
            if (file.CanWrite)
            {
                var stream = new StreamWriter(file);
                foreach (var data in articleDataList)
                {
                    stream.WriteLine($"~~~~~~~~~~~~~~~~~~[{data.Date.ToString("dd.MM.yyyy")}]~~~~~~~~~~~~~~~~~~");
                    
                    foreach(var article in data.Articles)
                    {
                        stream.WriteLine($"[{article.Time}] \"{article.Title}\" => https://www.tagesschau.de{article.Url}");
                    }

                    stream.WriteLine("");
                    stream.WriteLine("");
                }

                // Close StreamWriter
                stream.Close();
            }

            // Close FileStream
            file.Close();


            Console.WriteLine("Done!");

            Console.ReadLine();
        }
    }
}