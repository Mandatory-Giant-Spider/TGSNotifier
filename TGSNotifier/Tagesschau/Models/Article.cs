using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGSNotifier.Tagesschau.Models
{
    internal class Article
    {
        public string Url { get; }
        public string Title { get; }
        public string Time { get; }
        public Article(string url, string title, string time)
        {
            Url = url;
            Title = title;
            Time = time;
        }
    }
}
