using System;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class GherkinDialect : global::Gherkin.GherkinDialect
    {
        public string[] WhyKeywords = new string[0];

        public string[] WhoKeywords = new string[0];

        public string[] WhereKeywords = new string[0];

        public string[] WhatKeywords = new string[0];

        public string[] MetaKeywords = new string[0];

        public GherkinDialect(global::Gherkin.GherkinDialect dialect,
            DialectResource resource) : this(dialect.Language, dialect.FeatureKeywords, dialect.RuleKeywords, dialect.BackgroundKeywords,
            dialect.ScenarioKeywords, dialect.ScenarioOutlineKeywords, dialect.ExamplesKeywords, dialect.GivenStepKeywords,
            dialect.WhenStepKeywords, dialect.ThenStepKeywords, dialect.AndStepKeywords, dialect.ButStepKeywords, resource.KeyWords.Why, resource.KeyWords.Who, resource.KeyWords.Where, resource.KeyWords.What, resource.KeyWords.Meta)
        {

        }

        public GherkinDialect(global::Gherkin.GherkinDialect dialect,
            string[] whyKeywords, string[] whoKeywords, string[] whereKeywords,
            string[] whatKeywords, string[] metaKeywords) : this(dialect.Language, dialect.FeatureKeywords, dialect.RuleKeywords, dialect.BackgroundKeywords,
            dialect.ScenarioKeywords, dialect.ScenarioOutlineKeywords, dialect.ExamplesKeywords, dialect.GivenStepKeywords,
            dialect.WhenStepKeywords, dialect.ThenStepKeywords, dialect.AndStepKeywords, dialect.ButStepKeywords, whyKeywords, whoKeywords, whereKeywords, whatKeywords, metaKeywords)
        {

        }

        public GherkinDialect(string language, string[] featureKeywords, string[] ruleKeywords,
            string[] backgroundKeywords, string[] scenarioKeywords, string[] scenarioOutlineKeywords,
            string[] examplesKeywords, string[] givenStepKeywords, string[] whenStepKeywords,
            string[] thenStepKeywords, string[] andStepKeywords, string[] butStepKeywords,
            string[] whyKeywords, string[] whoKeywords, string[] whereKeywords, string[] whatKeywords, string[] metaKeywords) : base(language, featureKeywords, ruleKeywords, backgroundKeywords,
            scenarioKeywords, scenarioOutlineKeywords, examplesKeywords, givenStepKeywords,
            whenStepKeywords, thenStepKeywords, andStepKeywords, butStepKeywords)
        {
            this.WhyKeywords = whyKeywords;
            this.WhoKeywords = whoKeywords;
            this.WhereKeywords = whereKeywords;
            this.WhatKeywords = whatKeywords;
            this.MetaKeywords = metaKeywords;
        }
    }
}