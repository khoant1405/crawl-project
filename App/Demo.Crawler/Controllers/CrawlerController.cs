using Microsoft.AspNetCore.Mvc;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Text;
using Demo.Crawler.Common.Contants;
using Demo.CoreData.Models;

namespace Demo.Crawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly ILogger<CrawlerController> _logger;

        public CrawlerController(ILogger<CrawlerController> logger)
        {
            _logger = logger;
        }

        [Route("[action]")]
        [HttpGet]
        public IEnumerable<Article> StartCrawler()
        {
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            var listArticle = new List<Article>();
            var lisArticleContent = new List<ArticleContent>();

            for (int i = 1; i < 2; i++)
            {
                var document = web.Load($"{Contants.page}{i}");
                var articles = document.DocumentNode.QuerySelectorAll("article").ToList();

                foreach (var item in articles)
                {
                    var article = item.QuerySelector("div > a");

                    if (article != null)
                    {
                        string articleName = article.Attributes["title"].Value;
                        string href = article.Attributes["href"].Value;
                        string imageThumb = article.QuerySelector("picture > source > img").Attributes["src"].Value;
                        string description = item.QuerySelector("p.description > a").InnerHtml.Replace("\n", "");
                        DateTime time = DateTime.Now;

                        //var documentContent = web.Load(href);
                        //var data = documentContent.DocumentNode.QuerySelector("div.bbWrapper").InnerHtml.ToString();
                        //var indexOfFooter = data.IndexOf("<div class=\"k_sub_heading_div\">") - 1;
                        //var content = "";
                        //if (indexOfFooter > 0)
                        //{
                        //    content = data.Substring(0, indexOfFooter).Trim();
                        //}

                        var newArticle = new Article()
                        {
                            Id = Guid.NewGuid(),
                            ArticleName = articleName,
                            Status = "Publish",
                            CreationDate = time,
                            LastSaveDate = time,
                            CreationBy = Contants.idAdmin,
                            RefUrl = href,
                            ImageThumb = imageThumb,
                            Description = description
                        };
                        listArticle.Add(newArticle);

                        //var newArticleContent = new ArticleContent()
                        //{
                        //    Id = Guid.NewGuid(),
                        //    ArticleId = newArticle.Id,
                        //    Content = content,
                        //};
                        //lisArticleContent.Add(newArticleContent);
                    }
                }
            }
            return listArticle;
        }
    }
}