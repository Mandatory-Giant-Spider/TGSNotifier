using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGSNotifier.Tagesschau.Models
{
    internal class ArticleData
    {
        public List<Article> Articles { get; set; }
        public DateTime Date { get; set; }

        public ArticleData(List<Article> articles, DateTime date) 
        {
            Articles = articles;
            Date = date;
        }

    }
}
