using JSN.CoreData.Entities;
using JSN.CoreData.Models;

namespace JSN.CoreData.Repositories.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Article CreateNewArticle(Article article);
}