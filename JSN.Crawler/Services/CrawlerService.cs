using System.Net;
using AutoMapper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JSN.CoreData.Entities;
using JSN.CoreData.Models;
using JSN.CoreData.ViewModels;
using JSN.Crawler.Common;
using JSN.Crawler.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSN.Crawler.Services;

public class CrawlerService : ICrawlerService
{
    private readonly IRepository<ArticleContent> _articleContentRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CrawlerService(IUnitOfWork unitOfWork, IRepository<Article> articleRepository,
        IRepository<ArticleContent> articleContentRepository,
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _articleRepository = articleRepository;
        _articleContentRepository = articleContentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task StartCrawlerAsync(int startPage, int endPage)
    {
        var webClient = new GZipWebClient();
        var listArticle = new List<Article>();
        var lisArticleContent = new List<ArticleContent>();
        var listId = _articleRepository.List(x => x.Status == "Publish").AsNoTracking().Select(x => x.Id).ToList();

        for (var i = startPage; i < endPage + 1; i++)
        {
            var html = webClient.DownloadString($"{Constant.Page}{i}");
            var document = new HtmlDocument();
            document.LoadHtml(html);
            List<HtmlNode> articles = document.DocumentNode.QuerySelectorAll("div.zone--timeline > article").ToList();

            foreach (var item in articles)
            {
                var article = item.QuerySelector("a");

                if (article == null) continue;

                var articleId = int.Parse(item.Attributes["data-id"].Value);
                if (listId.Any(x => x == articleId) || listArticle.Any(x => x.Id == articleId)) continue;

                var articleName = article.Attributes["title"].Value;
                var href = article.Attributes["href"].Value;
                var imageThumb = article.QuerySelector("img")?.Attributes["data-src"].Value
                    .Replace("150x100", "200x150");
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
                    CategoryId = 12
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

        if (listArticle.Any())
        {
            _articleRepository.AddRange(listArticle);

            _articleContentRepository.AddRange(lisArticleContent);

            await _unitOfWork.CommitAsync();
        }
    }

    public async Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page, int pageSize)
    {
        try
        {
            var allArticles = _articleRepository.List(x => x.Status == "Publish").OrderByDescending(x => x.Id)
                .AsNoTracking();
            var count = allArticles.Count();
            var items = await allArticles.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => _mapper.Map<ArticleView>(x)).ToListAsync();
            foreach (var item in items)
            {
                item.UserName = _userRepository.List(x => x.Id == item.CreationBy).Select(x => x.UserName).FirstOrDefault();
            }
            return new PaginatedList<ArticleView>(items, count, page, pageSize);
        }
        catch (Exception)
        {
            return new PaginatedList<ArticleView>(null, 0, page, pageSize);
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