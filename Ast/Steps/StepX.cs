using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class StepX : Step
    {
        internal StepX(string name)
            : base(null, null, null, null)
        {
            this.Name = name;
        }

        public StepX(Step parent, string name)
            : base(parent.Location, parent.Keyword, parent.Text, parent.Argument)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}