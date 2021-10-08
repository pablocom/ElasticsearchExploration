using ElasticsearchExploration.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace ElasticsearchExploration.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManuscriptsController : ControllerBase
    {
        private static readonly IElasticClient elasticClient = BuildElasticClient();

        public ManuscriptsController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddBlogPost()
        {
            var manuscript = new Manuscript(Guid.NewGuid(), "Pharrel Williams", 4.20);
            var indexResponse = await elasticClient.IndexDocumentAsync(manuscript);

            return CreatedAtRoute(nameof(GetBlogPost), new { guid = manuscript.Guid }, manuscript);
        }

        [HttpGet("{guid}", Name = nameof(GetBlogPost))]
        public async Task<ActionResult<Manuscript>> GetBlogPost(Guid guid)
        {
            var queryResult = await elasticClient.SearchAsync<Manuscript>(s =>
                s.Query(q => q.Match(m => m.Field(f => f.Guid).Query(guid.ToString())))
            );
            return Ok(queryResult.Documents.First());
        }

        private static IElasticClient BuildElasticClient()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("manuscripts");
            return new ElasticClient(settings);
        }
    }
}
