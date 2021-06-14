using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public interface INarrative : IHasDescription, IHasLocation
    {
        new string Keyword { get; set; }

        new string Name { get; set; }

        new string Description { get; set; }

        new Location Location { get; set; }
    }

    public class Narrative : INarrative
    {
        public string Keyword { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public Location Location { get; set; } = new Location(0, 0);
    }
}