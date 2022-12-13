using Demo.CoreData.Models.View;
namespace Demo.Crawler.Services.Interfaces;
using Demo.Crawler.Common;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int? startPage, int? endPage);
    Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page);
}
