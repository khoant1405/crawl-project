using Demo.News.Services;
using Demo.News.Services.Interfaces;
using Demo.CoreData.Repositories;
using Demo.CoreData.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Demo.CoreData.Common;

namespace Demo.News.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<DemoDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("CoreData"));
                    options.UseLazyLoadingProxies();
                }
            );

            services.AddScoped<Func<DemoDbContext>>((provider) => () => provider.GetService<DemoDbContext>());
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
                .AddScoped<ICrawlerService, CrawlerService>();
        }
    }
}
