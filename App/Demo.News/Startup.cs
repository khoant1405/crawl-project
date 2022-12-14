using System.Text.Json.Serialization;
using Demo.News.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Demo.News
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddDatabase(Configuration);
            services.AddRepositories();
            services.AddServices();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //if (env.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

            //app.MapFallbackToFile("index.html");
        }
    }
}
