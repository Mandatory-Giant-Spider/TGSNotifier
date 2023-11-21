using HtmlAgilityPack;
using System.Text.RegularExpressions;

using TGSNotifier.Tagesschau.Models;

namespace TGSNotifier.Tagesschau
{
    internal class TagesschauParser
    {
        public DateTime Date { get; set; }
        public TagesschauParser()
        {
            Date = DateTime.Now.Date;
        }

        public TagesschauParser(DateTime dateTime)
        {
            Date = dateTime;
        }

        private List<Article> GetArticles()
        {
            List<Article> articleList = new List<Article>();

            {
                String date = Date.ToString("yyyy-MM-dd");
                String url = $"https://www.tagesschau.de/archiv/allemeldungen?datum={date}";

                var httpClient = new HttpClient();


                var html = httpClient.GetStringAsync(url).Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var articles = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"content\"]/div/div[2]/div/div/ul/div");
                foreach (var articleDiv in articles)
                {
                    string articleLink = "";
                    string articleTitle = "";
                    string articleTime = "";

                    HtmlNode urlTag = articleDiv.SelectSingleNode(".//li/a");
                    articleLink = urlTag.GetAttributeValue("href", "");

                    HtmlNode timeTag = urlTag.SelectSingleNode(".//div/p/span[@class='teaser-mikro__date']");
                    articleTime = timeTag.InnerText;

                    HtmlNode headlineTag = urlTag.SelectSingleNode(".//div/h3");
                    articleTitle = headlineTag.InnerText;

                    // Remove random hyphenate stuff
                    articleTitle = articleTitle.Replace("<span class=\"hyphenate\">", "");
                    articleTitle = articleTitle.Replace("</span>", "");

                    // Replace all grouped whitespaces
                    articleTitle = Regex.Replace(articleTitle, @"\s+", " ");

                    // Remove first and last whitespace
                    articleTitle = articleTitle.Substring(1, articleTitle.Length - 2);


                    articleList.Add(new(articleLink, articleTitle, articleTime));
                }
            }

            return articleList;
        }

        public ArticleData RequestData()
        {
            return new ArticleData(GetArticles(), Date);
        }
    }
}
