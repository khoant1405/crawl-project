using Demo.CoreData.Models.View;
namespace Demo.Crawler.Services.Interfaces;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int? startPage, int? endPage);
    IEnumerable<ArticleView> GetAllArticle();
}
