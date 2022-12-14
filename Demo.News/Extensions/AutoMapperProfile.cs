using Demo.CoreData.Models;
using AutoMapper;
using Demo.CoreData.Models.View;

namespace Demo.News.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Article, ArticleView>();
        }
    }
}
