﻿using ElasticsearchExploration.WebApi.Model;
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
        private static readonly IElasticClient ElasticClient = BuildElasticClient();

        [HttpPost]
        public async Task<IActionResult> AddManuscript()
        {
            var manuscript = new Manuscript(Guid.NewGuid(), "Pharrel Williams", 4.20);
            await ElasticClient.IndexDocumentAsync(manuscript);

            return CreatedAtRoute(nameof(GetManuscript), new { guid = manuscript.Guid }, manuscript);
        }

        [HttpGet("{guid}", Name = nameof(GetManuscript))]
        public async Task<ActionResult<Manuscript>> GetManuscript(Guid guid)
        {
            var query = new MatchQuery
            {
                Field = Infer.Field<Manuscript>(m => m.Guid),
                Query = guid.ToString()
            };
            var queryResult = await ElasticClient.SearchAsync<Manuscript>(new SearchRequest {Query = query});

            return Ok(queryResult.Documents.First());
        }

        private static IElasticClient BuildElasticClient()
        {
            return new ElasticClient(new ConnectionSettings().DefaultIndex("manuscripts"));
        }
    }
}
