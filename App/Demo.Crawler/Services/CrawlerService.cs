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
using System.Net;

namespace Demo.Crawler.Services
{
    public class CrawlerService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IArticleRepository _articleRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticleContent> _articleContentRepository;

        public class GZipWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return request;
            }
        }

        //public CrawlerService(IUnitOfWork unitOfWork, IArticleRepository articleRepository)
        public CrawlerService(IUnitOfWork unitOfWork, IRepository<Article> articleRepository, IRepository<ArticleContent> articleContentRepository)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
            _articleContentRepository = articleContentRepository;
        }

        public async Task<IEnumerable<Article>> StartCrawlerAsync()
        {
            string html;
            string htmlArticle;
            GZipWebClient webClient = new GZipWebClient();
            List<Article> listArticle = new List<Article>();
            List<ArticleContent> lisArticleContent = new List<ArticleContent>();

            for (int i = 13; i < 15; i++)
            {
                html = webClient.DownloadString($"{Contants.page}{i}");
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                List<HtmlNode> articles = document.DocumentNode.QuerySelectorAll("div.zone--timeline > article").ToList();

                foreach (HtmlNode item in articles)
                {
                    HtmlNode article = item.QuerySelector("a");

                    if (article != null)
                    {
                        Guid articleId = Guid.NewGuid();
                        string articleName = article.Attributes["title"].Value;
                        string href = article.Attributes["href"].Value;
                        string? imageThumb = article.QuerySelector("img")?.Attributes["data-src"].Value;
                        string? description = item.QuerySelector("div > div.summary > p")?.InnerText.Replace("\n", "");
                        string? time = item.QuerySelector("div > div.meta > span.time")?.InnerText.Replace("\n", "");
                        DateTime dateTime = new DateTime();
                        if (time != null)
                        {
                            dateTime = DateTime.ParseExact(time, "HH:mm dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }

                        htmlArticle = webClient.DownloadString(href);
                        HtmlDocument documentArticle = new HtmlDocument();
                        documentArticle.LoadHtml(htmlArticle);
                        string? content = documentArticle.DocumentNode.QuerySelector("div.cms-body")?.OuterHtml;

                        Article newArticle = new Article()
                        {
                            Id = articleId,
                            ArticleName = articleName,
                            Status = "Publish",
                            CreationDate = dateTime,
                            LastSaveDate = dateTime,
                            CreationBy = Contants.idAdmin,
                            RefUrl = href,
                            ImageThumb = imageThumb,
                            Description = description,
                            CategoryId = 14,
                            Page = i
                        };
                        listArticle.Add(newArticle);

                        ArticleContent newArticleContent = new ArticleContent()
                        {
                            Id = Guid.NewGuid(),
                            Content = content,
                            ArticleId = articleId
                        };
                        lisArticleContent.Add(newArticleContent);
                    }
                }
            }

            _articleRepository.AddRange(listArticle);

            _articleContentRepository.AddRange(lisArticleContent);

            var saved = await _unitOfWork.CommitAsync();

            return listArticle;
        }
    }
}
