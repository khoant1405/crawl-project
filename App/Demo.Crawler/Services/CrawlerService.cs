using Demo.CoreData.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Demo.Crawler.Common.Contants;
using System.Net;
using Demo.Crawler.Services.Interfaces;
using Demo.CoreData.Common;
using Demo.CoreData.Models.View;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Demo.Crawler.Common;

namespace Demo.Crawler.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly DemoDbContext _demoDbContext;
        //private readonly IArticleRepository _articleRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticleContent> _articleContentRepository;
        private readonly IMapper _mapper;

        public class GZipWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return request;
            }
        }

        public CrawlerService(IUnitOfWork unitOfWork, IRepository<Article> articleRepository, IRepository<ArticleContent> articleContentRepository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
            _articleContentRepository = articleContentRepository;
            _mapper = mapper;
        }

        public async Task StartCrawlerAsync(int? startPage, int? endPage)
        {
            string html;
            string htmlArticle;
            GZipWebClient webClient = new GZipWebClient();
            List<Article> listArticle = new List<Article>();
            List<ArticleContent> lisArticleContent = new List<ArticleContent>();

            for (int? i = startPage; i < endPage + 1; i++)
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
                        int idDisplay = int.Parse(item.Attributes["data-id"].Value);
                        string articleName = article.Attributes["title"].Value;
                        string href = article.Attributes["href"].Value;
                        string? imageThumb = article.QuerySelector("img")?.Attributes["data-src"].Value;
                        string? description = item.QuerySelector("div > div.summary > p")?.InnerText.Replace("\n", "");

                        htmlArticle = webClient.DownloadString(href);
                        HtmlDocument documentArticle = new HtmlDocument();
                        documentArticle.LoadHtml(htmlArticle);
                        string? content = documentArticle.DocumentNode.QuerySelector("div.cms-body")?.OuterHtml;
                        string? time = documentArticle.DocumentNode.QuerySelector("meta.cms-date")?.Attributes["content"].Value;
                        DateTime dateTime = new DateTime();
                        if (time != null)
                        {
                            dateTime = DateTime.Parse(time);
                        }

                        Article newArticle = new Article()
                        {
                            Id = articleId,
                            ArticleName = articleName,
                            Status = "Publish",
                            CreationDate = dateTime,
                            CreationBy = Contants.idAdmin,
                            RefUrl = href,
                            ImageThumb = imageThumb,
                            Description = description,
                            CategoryId = 12,
                            IdDisplay = idDisplay
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

            await _unitOfWork.CommitAsync();
        }

        public async Task<ArticlePagination> GetAllArticleAsync(int page)
        {
            int pageSize = 16;
            var allArticles = _articleRepository.List(x => x.Status == "Publish").OrderByDescending(x => x.CreationDate).AsNoTracking();
            var count = await allArticles.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var hasPreviousPage = page > 1;
            var hasNextPage = page < totalPages;
            var items = await allArticles.Skip((page - 1) * pageSize).Take(pageSize).Select(x => _mapper.Map<ArticleView>(x)).ToListAsync();
            //var showFromPage = page - 2 > 0 ? page - 2 : page - 1;
            return new ArticlePagination
            {
                PageIndex = page,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                Data = items
            };
        }
    }
}