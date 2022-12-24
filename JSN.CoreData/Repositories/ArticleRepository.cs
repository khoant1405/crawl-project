using JSN.CoreData.Entities;
using JSN.CoreData.Models;
using JSN.CoreData.Repositories.Interfaces;

namespace JSN.CoreData.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public Article CreateNewArticle(Article article)
    {
        Add(article);
        return article;
    }
}