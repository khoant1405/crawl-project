using Demo.CoreData.Models;

namespace Demo.Crawler.Services.Interfaces;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int? startPage, int? endPage);
    IEnumerable<Article> GetAllArticle();
}
