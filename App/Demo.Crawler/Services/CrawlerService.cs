using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.CoreData;
using Demo.CoreData.Repositories.Interfaces;
using Demo.CoreData.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Text;
using Demo.Crawler.Common.Contants;

namespace Demo.Crawler.Services
{
    public class CrawlerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleRepository _articleRepository;

        public CrawlerService(IUnitOfWork unitOfWork, IArticleRepository articleRepository)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> StartCrawlerAsync()
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

                        _articleRepository.CreateNewArticle(newArticle);
                    }
                }
            }

            var saved = await _unitOfWork.CommitAsync();

            return listArticle;
        }
    }
}
