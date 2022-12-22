using System.Net;
using AutoMapper;
using Demo.CoreData.Entities;
using Demo.CoreData.Models;
using Demo.CoreData.ViewModels;
using Demo.Crawler.Common;
using Demo.Crawler.Services.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace Demo.Crawler.Services;

public class CrawlerService : ICrawlerService
{
    private readonly IRepository<ArticleContent> _articleContentRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CrawlerService(IUnitOfWork unitOfWork, IRepository<Article> articleRepository,
        IRepository<ArticleContent> articleContentRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _articleRepository = articleRepository;
        _articleContentRepository = articleContentRepository;
        _mapper = mapper;
    }

    public async Task StartCrawlerAsync(int startPage, int endPage)
    {
        var webClient = new GZipWebClient();
        var listArticle = new List<Article>();
        var lisArticleContent = new List<ArticleContent>();

        for (var i = startPage; i < endPage + 1; i++)
        {
            var html = webClient.DownloadString($"{Constant.Page}{i}");
            var document = new HtmlDocument();
            document.LoadHtml(html);
            List<HtmlNode> articles = document.DocumentNode.QuerySelectorAll("div.zone--timeline > article").ToList();

            foreach (var item in articles)
            {
                var article = item.QuerySelector("a");

                if (article != null)
                {
                    var articleId = Guid.NewGuid();
                    var idDisplay = int.Parse(item.Attributes["data-id"].Value);
                    var articleName = article.Attributes["title"].Value;
                    var href = article.Attributes["href"].Value;
                    var imageThumb = article.QuerySelector("img")?.Attributes["data-src"].Value;
                    var description = item.QuerySelector("div > div.summary > p")?.InnerText.Replace("\n", "");

                    var htmlArticle = webClient.DownloadString(href);
                    var documentArticle = new HtmlDocument();
                    documentArticle.LoadHtml(htmlArticle);
                    var content = documentArticle.DocumentNode.QuerySelector("div.cms-body")?.OuterHtml;
                    var time = documentArticle.DocumentNode.QuerySelector("meta.cms-date")?.Attributes["content"].Value;
                    var dateTime = new DateTime();
                    if (time != null) dateTime = DateTime.Parse(time);

                    var newArticle = new Article
                    {
                        Id = articleId,
                        ArticleName = articleName,
                        Status = "Publish",
                        CreationDate = dateTime,
                        CreationBy = Constant.IdAdmin,
                        RefUrl = href,
                        ImageThumb = imageThumb,
                        Description = description,
                        CategoryId = 12,
                        IdDisplay = idDisplay
                    };
                    listArticle.Add(newArticle);

                    ArticleContent newArticleContent = new()
                    {
                        Id = Guid.NewGuid(),
                        Content = content,
                        ArticleId = articleId
                    };
                    lisArticleContent.Add(newArticleContent);
                }
            }
        }

        var listArticleDistinct = listArticle.DistinctBy(x => x.IdDisplay).ToList();
        var lisArticleContentDistinct =
            lisArticleContent.Where(x => listArticleDistinct.Exists(y => y.Id == x.ArticleId)).ToList();

        _articleRepository.AddRange(listArticleDistinct);

        _articleContentRepository.AddRange(lisArticleContentDistinct);

        await _unitOfWork.CommitAsync();
    }

    public async Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page, int pageSize)
    {
        try
        {
            var allArticles = _articleRepository.List(x => x.Status == "Publish").OrderByDescending(x => x.CreationDate)
                .AsNoTracking();
            var count = await allArticles.CountAsync();
            var items = await allArticles.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => _mapper.Map<ArticleView>(x)).ToListAsync();
            return new PaginatedList<ArticleView>(items, count, page, pageSize, Constant.NumberOfPagesShow);
        }
        catch (Exception)
        {
            return new PaginatedList<ArticleView>(null, 0, page, pageSize, Constant.NumberOfPagesShow);
        }
    }

    public class GZipWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }
}