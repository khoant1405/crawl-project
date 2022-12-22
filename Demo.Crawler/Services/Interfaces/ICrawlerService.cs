using Demo.CoreData.ViewModels;
using Demo.Crawler.Common;

namespace Demo.Crawler.Services.Interfaces;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int startPage, int endPage);
    Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page, int pageSize);
}