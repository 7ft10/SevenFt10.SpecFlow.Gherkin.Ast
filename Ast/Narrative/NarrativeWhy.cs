namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class NarrativeWhy : Narrative {

        public new const string Keyword = "Why";

        public NarrativeWhy()
        {
            base.Keyword = NarrativeWhy.Keyword;
        }
     }

    public class NarrativeButWhy : NarrativeWhy { }

    public class NarrativeAndWhy : NarrativeWhy { }
}