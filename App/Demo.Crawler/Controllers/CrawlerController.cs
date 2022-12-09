using Microsoft.AspNetCore.Mvc;
using Demo.CoreData.Models;
using Demo.Crawler.Services;

namespace Demo.Crawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly ILogger<CrawlerController> _logger;
        private readonly CrawlerService _crawlerService;

        public CrawlerController(CrawlerService crawlerService, ILogger<CrawlerController> logger)
        {
            _logger = logger;
            _crawlerService = crawlerService;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<Article>> StartCrawlerAsync()
        {
            var listArticle = await _crawlerService.StartCrawlerAsync();
            return listArticle;
        }
    }
}