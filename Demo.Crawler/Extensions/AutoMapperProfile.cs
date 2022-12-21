using Demo.CoreData.Models;
using AutoMapper;
using Demo.CoreData.ViewModels;

namespace Demo.Crawler.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Article, ArticleView>();
        }
    }
}
