using Demo.CoreData.Models.View;
namespace Demo.News.Services.Interfaces;
using Demo.News.Common;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int? startPage, int? endPage);
    Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page, int pageSize);
}
