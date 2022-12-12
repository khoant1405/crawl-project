﻿using System;
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
using Demo.Crawler.Services.Interfaces;
using Demo.CoreData.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Demo.Crawler.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DemoDbContext _demoDbContext;
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
        public CrawlerService(IUnitOfWork unitOfWork, IRepository<Article> articleRepository, IRepository<ArticleContent> articleContentRepository,
                                DemoDbContext demoDbContext)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
            _articleContentRepository = articleContentRepository;
            _demoDbContext = demoDbContext;
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
                            LastSaveDate = dateTime,
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

            var saved = await _unitOfWork.CommitAsync();
        }

        public IEnumerable<Article> GetAllArticle()
        {
            var articles = _articleRepository.List(x => x.Status == "Publish").OrderByDescending(x=>x.IdDisplay).ToList();
            //var articles = _demoDbContext.Articles.AsNoTracking().Where(x => x.Status == "Publish").Include(x=>x.ArticleContents);
            return articles;
        }
    }
}