using JSN.CoreData.Entities;
using JSN.CoreData.Repositories;
using JSN.CoreData.Repositories.Interfaces;
using JSN.Crawler.Services;
using JSN.Crawler.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSN.Crawler.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext with Scoped lifetime
        services.AddDbContext<JSNDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CoreData"));
                options.UseLazyLoadingProxies();
            }
        );

        services.AddScoped<Func<JSNDbContext>>(provider => () => provider.GetService<JSNDbContext>()!);
        services.AddScoped<DbFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IArticleRepository, ArticleRepository>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICrawlerService, CrawlerService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>();
    }
}