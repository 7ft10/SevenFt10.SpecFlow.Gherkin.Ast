namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class NarrativeWhat : Narrative
    {
        public new const string Keyword = "What";

        public NarrativeWhat()
        {
            base.Keyword = NarrativeWhat.Keyword;
        }
    }

    public class NarrativeButWhat : NarrativeWhat { }

    public class NarrativeAndWhat : NarrativeWhat { }
}