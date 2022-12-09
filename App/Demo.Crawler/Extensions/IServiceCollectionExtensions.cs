using Demo.Crawler.Services;
using Demo.CoreData.Models;
using Demo.CoreData.Repositories;
using Demo.CoreData.Repositories.Interfaces;
using Demo.CoreData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Crawler.Extensions
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
                .AddScoped<CrawlerService>();
        }
    }
}
