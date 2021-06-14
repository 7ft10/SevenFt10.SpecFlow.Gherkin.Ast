using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public static class ReplacementFactory
    {
        public static IHasLocation ReplaceSteps(this IHasLocation item, IEnumerable<Step> newSteps)
        {
            if (item is Scenario)
            {
                var topScenario = (Scenario)item;
                return new Scenario(
                    topScenario.Tags.ToArray(),
                    topScenario.Location,
                    topScenario.Keyword,
                    topScenario.Name,
                    topScenario.Description,
                    newSteps.ToArray(),
                    topScenario.Examples.ToArray()
                );
            }
            else if (item is Background)
            {
                var topScenario = (Background)item;
                return new Background(
                    topScenario.Location,
                    topScenario.Keyword,
                    topScenario.Name,
                    topScenario.Description,
                    newSteps.ToArray()
                );
            }
            return item;
        }

        public static IHasChildren ReplaceChildren(this IHasChildren top, IEnumerable<IHasLocation> newChilds)
        {
            if (top is Feature)
            {
                var feature = (Feature)top;
                return new Feature(
                    feature.Tags.ToArray(),
                    feature.Location,
                    feature.Language,
                    feature.Keyword,
                    feature.Name,
                    feature.Description,
                    newChilds.ToArray()
                );
            }
            else if (top is Rule)
            {
                var rule = (Rule)top;
                return new Rule(
                    rule.Location,
                    rule.Keyword,
                    rule.Name,
                    rule.Description,
                    newChilds.ToArray()
                );
            }
            return top;
        }
    }
}