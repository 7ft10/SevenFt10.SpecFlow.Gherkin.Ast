using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Newtonsoft.Json;

[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.Satellite)]

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class DialectResource
    {
        public string Language = String.Empty;

        public KeyWords KeyWords = new KeyWords();

        internal static DialectResource GetDialectFromResource(string language)
        {
            try
            {
                var embeddedResource = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(s => s.EndsWith("." + language.Replace("-", "_") + ".gherkin_dialect.json"));

                if (!string.IsNullOrWhiteSpace(embeddedResource))
                {
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource))
                    using (var reader = new StreamReader(stream))
                    {
                        var dialect = JsonConvert.DeserializeObject<DialectResource>(reader.ReadToEnd().ToString());
                        if (dialect.Language == language)
                        {
                            return dialect;
                        }
                    }
                }

                throw new InvalidOperationException("Embedded dialect resource not found.");

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not load dialect from resource.", ex);
            }
        }
    }

    public class KeyWords
    {
        public string[] Why = new string[0];
        public string[] Who = new string[0];
        public string[] Where = new string[0];
        public string[] What = new string[0];
        public string[] Meta = new string[0];
    }
}