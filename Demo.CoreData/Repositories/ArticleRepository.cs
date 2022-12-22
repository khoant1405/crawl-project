using Demo.CoreData.Entities;
using Demo.CoreData.Models;
using Demo.CoreData.Repositories.Interfaces;

namespace Demo.CoreData.Repositories;

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