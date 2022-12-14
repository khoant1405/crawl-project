using Demo.CoreData.Common;
using Demo.CoreData.Models;

namespace Demo.CoreData.Repositories.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Article CreateNewArticle(Article article);
    }
}
