namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class NarrativeWho : Narrative
    {
        public new const string Keyword = "Who";

        public NarrativeWho()
        {
            base.Keyword = NarrativeWho.Keyword;
        }
    }

    public class NarrativeButWho : NarrativeWho { }

    public class NarrativeAndWho : NarrativeWho { }
}