using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gherkin.Ast;
using g = Gherkin;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    using static Constants;

    public static class OutputExtensions
    {
        private static string TrimGherkins(this string str)
        {
            return str.TrimStart(new char[] { '#' }).TrimStart(new char[] { '@' }); ;
        }

        public static string ToOutput(this GherkinDocument document)
        {
            StringBuilder sb = new StringBuilder();

            if (document == null || document.Feature == null)
            {
                return "Invalid document or feature";
            }

            if (document.Feature.Tags != null && document.Feature.Tags.Any())
            {
                sb.AppendLine($"{document.Feature.Tags.ToOutput()}");
            }
            sb.AppendLine($"{document.Feature.Keyword}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR} {document.Feature.Name}".Trim());

            if (document.Comments != null && document.Comments.Any())
            {
                sb.AppendLine(document.Comments.ToOutput());
            }

            sb.AppendLine(document.Feature.ToOutput());

            return FixEndDocumentNewLines(sb);
        }

        private static string FixEndDocumentNewLines(StringBuilder sb)
        {
            var str = sb.ToString().Replace("\t", SPACES);
            while(str.Contains(Environment.NewLine + Environment.NewLine + Environment.NewLine))
            {
                str = str.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
            }
            return str.TrimEnd(Environment.NewLine.ToCharArray()) + Environment.NewLine;
        }

        public static string ToOutput(this Feature feature)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{feature.Description}".Trim());

            if (feature.Children != null)
            {
                if (feature.Children.OfType<Rule>().Any())
                {
                    foreach (var rule in feature.Children.OfType<Rule>())
                    {
                        sb.Append(rule.ToOutput());
                        sb.AppendLine();
                    }
                }
                else if (feature.Children.OfType<Scenario>().Any())
                {
                    foreach (var scenario in feature.Children.OfType<Scenario>())
                    {
                        sb.Append($"{scenario.ToOutput()}");
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }

        public static string ToOutput(this IEnumerable<Comment> comments)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var comment in comments)
            {
                sb.AppendLine($"{comment.ToOutput()}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this Location location)
        {
            StringBuilder sb = new StringBuilder();

            if (location != null)
            {
                sb.Append($"{location.Line}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this Comment comment)
        {
            StringBuilder sb = new StringBuilder();

            if (comment != null && !String.IsNullOrEmpty(comment.Text))
            {
                sb.Append($"{g.GherkinLanguageConstants.COMMENT_PREFIX} {comment.Text.TrimGherkins()}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this IEnumerable<RuleMeta> metas)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var meta in metas)
            {
                sb.Append($"\t\t{meta.ToOutput()}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this IEnumerable<Narrative> narratives)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var narrative in narratives)
            {
                sb.AppendLine($"\t\t{narrative.ToOutput()}");
            }

            return sb.ToString().Trim();
        }

        public static string ToOutput(this IEnumerable<Scenario> scenarios)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var scenario in scenarios)
            {
                sb.Append($"\t\t{scenario.ToOutput()}");
            }

            return sb.ToString().Trim();
        }

        public static string ToOutput(this IEnumerable<Examples> examples)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var example in examples)
            {
                sb.Append($"{example.ToOutput()}");
            }
            sb.AppendLine();

            return sb.ToString().Trim();
        }

        public static string ToOutput(this IEnumerable<Background> backgrounds)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var background in backgrounds)
            {
                sb.Append($"{background.ToOutput()}");
            }
            sb.AppendLine();

            return sb.ToString().Trim();
        }

        public static string ToOutput(this Rule rule)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{rule.Keyword}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR} {rule.Name}".Trim());

            if (rule.Children != null)
            {
                if (rule.Children.OfType<RuleMeta>().Any() || rule.Children.OfType<Narrative>().Any())
                {
                    if (rule.Children.OfType<RuleMeta>().Any())
                    {
                        sb.AppendLine($"\t{META_BOUNDARY}");
                        sb.Append($"{rule.Children.OfType<RuleMeta>().ToOutput()}");
                        sb.AppendLine($"\t{META_BOUNDARY}");
                    }

                    if (rule.Children.OfType<Narrative>().Any())
                    {
                        sb.AppendLine($"\t{rule.Children.OfType<Narrative>().ToOutput()}{Environment.NewLine}");
                    }
                }
                else
                {
                    sb.AppendLine($"\t{rule.Description}");
                }

                if (rule.Children.OfType<Scenario>().Any())
                {
                    sb.AppendLine($"{rule.Children.OfType<Scenario>().ToOutput()}");
                }
                if (rule.Children.OfType<Background>().Any())
                {
                    sb.AppendLine($"{rule.Children.OfType<Background>().ToOutput()}");
                }
                if (rule.Children.OfType<Examples>().Any())
                {
                    sb.AppendLine($"{rule.Children.OfType<Examples>().ToOutput()}");
                }
            }
            else
            {
                sb.AppendLine($"\t{rule.Description}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this Narrative narrative)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{narrative.Name.Trim()} {narrative.Description.Trim()}");

            return sb.ToString();
        }

        public static string ToOutput(this RuleMeta meta)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{meta.Name}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR} {meta.Text}".Trim());

            return sb.ToString();
        }

        public static string ToOutput(this Background background)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"\t{background.Keyword}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR} {background.Name}");

            sb.AppendLine($"\t{background.Description}");

            foreach (var step in background.Steps)
            {
                sb.Append($"\t\t{step.ToOutput()}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this IEnumerable<Tag> tags)
        {
            StringBuilder sb = new StringBuilder();

            if (tags != null && tags.Any())
            {
                sb.Append($"\t{String.Join(" ", tags.Select(t => g.GherkinLanguageConstants.TAG_PREFIX + t.Name.TrimGherkins()))}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string ToOutput(this Scenario scenario)
        {
            StringBuilder sb = new StringBuilder();

            if (scenario.Tags != null && scenario.Tags.Any())
            {
                sb.AppendLine(scenario.Tags.ToOutput());
            }

            if (!String.IsNullOrEmpty(scenario.Description) || !String.IsNullOrEmpty(scenario.Keyword) || !String.IsNullOrEmpty(scenario.Name))
            {
                if (!String.IsNullOrEmpty(scenario.Keyword))
                {
                    sb.Append($"\t{scenario.Keyword}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR}");
                }
                if (!String.IsNullOrEmpty(scenario.Name))
                {
                    sb.Append($"{scenario.Name}");
                }
                sb.AppendLine();

                if (!String.IsNullOrEmpty(scenario.Description))
                {
                    sb.AppendLine($"\t{scenario.Description.Trim()}");
                }
            }

            sb.Append($"{scenario.Steps.ToOutput()}");

            if (scenario.Examples != null && scenario.Examples.Any())
            {
                sb.AppendLine($"{scenario.Examples.ToOutput()}");
            }

            return sb.ToString();
        }

        public static string ToOutput(this IEnumerable<Step> steps)
        {
            StringBuilder sb = new StringBuilder();

            if (steps != null)
            {
                foreach (var step in steps)
                {
                    sb.Append($"\t\t{step.ToOutput()}");
                }
            }

            return sb.ToString();
        }

        public static string ToOutput(this Step step)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{step.Keyword.Trim()} {step.Text}".Trim());
            if ((step.Argument is DataTable))
            {
                StringBuilder bs = new StringBuilder();
                DataTable arg = (DataTable)step.Argument;
                foreach (var row in arg.Rows)
                {
                    bs.Append(g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR);
                    foreach (var cell in row.Cells)
                    {
                        bs.Append($"{cell.Value} {g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR}");
                    }
                    bs.AppendLine();
                }
                var lines = bs.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (lines.Any())
                {
                    sb.AppendLine("\t\t" + FormatGherkinTable(lines).Replace(Environment.NewLine, Environment.NewLine + "\t\t"));
                }
            }
            else if ((step.Argument is DocString))
            {
                DocString arg = (DocString)step.Argument;
                if (!String.IsNullOrEmpty(arg.Content))
                {
                    sb.AppendLine($"\t\t{g.GherkinLanguageConstants.DOCSTRING_ALTERNATIVE_SEPARATOR}");
                    var lines = arg.Content.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Any())
                    {
                        foreach (var line in lines)
                        {
                            sb.AppendLine($"\t\t  {line}");
                        }
                    }
                    sb.AppendLine($"\t\t{g.GherkinLanguageConstants.DOCSTRING_ALTERNATIVE_SEPARATOR}");
                }
            }

            return sb.ToString();
        }

        public static string ToOutput(this Examples example, string prefix = "\t\t")
        {
            StringBuilder sb = new StringBuilder();

            if (example.Tags != null && example.Tags.Any())
            {
                sb.AppendLine(example.Tags.ToOutput());
            }

            sb.AppendLine($"\t{example.Keyword}{g.GherkinLanguageConstants.TITLE_KEYWORD_SEPARATOR} {example.Name}".Trim());

            StringBuilder bs = new StringBuilder();

            if (example.TableHeader != null && example.TableHeader.Cells != null && example.TableHeader.Cells.Any())
            {
                bs.Append($"{prefix}{g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR}\t");
                foreach (var th in example.TableHeader.Cells)
                {
                    bs.Append(th.Value + $"\t\t{g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR}\t");
                }
                bs.AppendLine();
            }

            if (example.TableBody != null && example.TableBody.Any())
            {
                foreach (var tb in example.TableBody)
                {
                    bs.Append($"{prefix}{g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR}\t");
                    foreach (var cell in tb.Cells)
                    {
                        bs.Append(cell.Value + $"\t\t{g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR}\t");
                    }
                    bs.AppendLine();
                }
            }

            var lines = bs.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines.Any())
            {
                sb.AppendLine(FormatGherkinTable(lines));
            }

            return sb.ToString();
        }

        private static string FormatGherkinTable(string[] lines)
        {
            var widths = Enumerable.Range(0, lines.First().Split(g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR.ToCharArray()).Count()).Select(c => 1).ToList();
            foreach (var line in lines)
            {
                var cols = line.Split(g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR.ToCharArray());
                for (var i = 0; i < cols.Length; i++)
                {
                    if (cols[i].Length > 0 && widths.ElementAtOrDefault(i) < cols[i].Trim().Length)
                    {
                        widths[i] = cols[i].Trim().Length;
                    }
                }
            }

            var newLines = new List<string>();
            foreach (var line in lines)
            {
                var cols = line.Split(g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR.ToCharArray());
                for (var i = 0; i < cols.Length; i++)
                {
                    cols[i] = cols[i].Trim() + new string(' ', widths[i] - cols[i].Trim().Length + 1);
                }
                newLines.Add("\t" + String.Join(g.GherkinLanguageConstants.TABLE_CELL_SEPARATOR + " ", cols).Trim());
            }

            return String.Join(Environment.NewLine, newLines.ToArray());
        }
    }
}