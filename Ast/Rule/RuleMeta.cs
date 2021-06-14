using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class RuleMeta : IHasLocation
    {
        public RuleMeta(Location location, string name, string text)
        {
            this.Location = location;
            this.Text = text;
            this.Name = name;
        }

        public string Text { get; }

        public string Name { get; }

        public Location Location { get; }
    }
}