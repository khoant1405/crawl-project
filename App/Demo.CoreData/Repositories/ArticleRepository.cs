using Demo.CoreData.Models;
using Demo.CoreData.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.CoreData.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public Article CreateNewArticle(Article Article)
        {
            this.Add(Article);
            return Article;
        }
    }
}
