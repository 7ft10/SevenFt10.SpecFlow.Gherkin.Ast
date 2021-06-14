using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public static class AstExtensions
    {
        public static IEnumerable<GherkinDocument> FilterByTags(this IEnumerable<GherkinDocument> documents, string[] includeTags)
        {
            foreach (var doc in documents)
            {
                if (includeTags == null || includeTags.Length == 0 || doc.Feature.Tags.Any(t => includeTags.Contains(t.Name)))
                {
                    yield return doc;
                }
            }
        }
    }
}