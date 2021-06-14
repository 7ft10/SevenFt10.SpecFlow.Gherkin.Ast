using System;
using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class GherkinDialectProvider
    {
        public static GherkinDialect GetFactoryDefault()
        {
            return new GherkinDialectProvider().DefaultDialect;
        }

        public GherkinDialect DefaultDialect => defaultDialect.Value;

        private Lazy<GherkinDialect> defaultDialect;

        private global::Gherkin.GherkinDialectProvider Provider { get; set; }

        public GherkinDialectProvider(string defaultLanguage = "en")
        {
            Provider = new global::Gherkin.GherkinDialectProvider(defaultLanguage);

            defaultDialect = new Lazy<GherkinDialect>(() =>
            {
                if (defaultLanguage == Provider.DefaultDialect.Language)
                {
                    var dialect = DialectResource.GetDialectFromResource(defaultLanguage);
                    return new GherkinDialect(Provider.DefaultDialect, dialect);
                }
                throw new global::Gherkin.NoSuchLanguageException(defaultLanguage, new Location());
            });
        }

        internal GherkinDialect GetDialect(string language, Location location)
        {
            if (language == DefaultDialect.Language)
            {
                return DefaultDialect;
            }
            try
            {
                var dialect = DialectResource.GetDialectFromResource(language);
                return new GherkinDialect(Provider.GetDialect(language, location), dialect);
            }
            catch
            {
                throw new global::Gherkin.NoSuchLanguageException(language, location);
            }
        }
    }
}