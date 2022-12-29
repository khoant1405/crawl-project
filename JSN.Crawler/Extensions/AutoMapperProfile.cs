using AutoMapper;
using JSN.CoreData.Models;
using JSN.CoreData.ViewModels;

namespace JSN.Crawler.Extensions;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Article, ArticleView>().ForMember(des => des.UserName, act => act.MapFrom(src => src.User.UserName));
    }
}