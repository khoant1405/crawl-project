using Microsoft.AspNetCore.Mvc;
using Demo.Crawler.Services.Interfaces;
using System.Net;
using Demo.CoreData.Models.View;

namespace Demo.Crawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly ILogger<CrawlerController> _logger;
        private readonly ICrawlerService _crawlerService;

        public CrawlerController(ICrawlerService crawlerService, ILogger<CrawlerController> logger)
        {
            _logger = logger;
            _crawlerService = crawlerService;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> StartCrawler(int? startPage, int? endPage)
        {
            if (startPage == null || endPage == null)
            {
                return BadRequest("Invalid Page");
            }
            await _crawlerService.StartCrawlerAsync(startPage, endPage);
            return Ok();
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArticleView>), (int)HttpStatusCode.OK)]
        public ActionResult GetAllArticle()
        {
            IEnumerable<ArticleView> articles = _crawlerService.GetAllArticle();
            return Ok(articles);
        }
    }
}