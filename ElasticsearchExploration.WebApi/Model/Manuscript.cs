using System;

namespace ElasticsearchExploration.WebApi.Model
{
    public class Manuscript
    {
        public Guid Guid { get; }
        public string AuthorName { get; }
        public double ImpactFactor { get; }

        public Manuscript(Guid guid, string authorName, double impactfactor)
        {
            Guid = guid;
            AuthorName = authorName;
            ImpactFactor = impactfactor;
        }
    }
}
