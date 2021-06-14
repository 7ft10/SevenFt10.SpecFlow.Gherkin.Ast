namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class NarrativeWhere : Narrative
    {
        public new const string Keyword = "Where";

        public NarrativeWhere()
        {
            base.Keyword = NarrativeWhere.Keyword;
        }
    }

    public class NarrativeButWhere : NarrativeWhy { }

    public class NarrativeAndWhere : NarrativeWhy { }
}