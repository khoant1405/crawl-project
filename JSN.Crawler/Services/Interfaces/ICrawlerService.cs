using JSN.CoreData.ViewModels;
using JSN.Crawler.Common;

namespace JSN.Crawler.Services.Interfaces;

public interface ICrawlerService
{
    Task StartCrawlerAsync(int startPage, int endPage);
    Task<PaginatedList<ArticleView>> GetArticleFromPageAsync(int page, int pageSize);
}