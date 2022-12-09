using Demo.CoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.CoreData.Repositories.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Article CreateNewArticle(Article article);
    }
}
