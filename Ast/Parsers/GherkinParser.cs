using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin;
using Gherkin.Ast;

namespace SevenFt10.SpecFlow.Gherkin.Ast
{
    public class GherkinParser : Parser
    {
        public GherkinParser() : base() { }

        public new GherkinDocument Parse(TextReader reader)
        {
            return ReparseDocument(base.Parse(reader));
        }

        public new GherkinDocument Parse(string sourceFile)
        {
            using (var fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                return ReparseDocument(base.Parse(textReader));
            }
        }

        private GherkinDialect GetDialect(GherkinDocument doc)
        {
            var provider = new GherkinDialectProvider(doc.Feature.Language);
            return provider.GetDialect(doc.Feature.Language, doc.Feature.Location);
        }

        private Step[] UpdateStepsX(IHasSteps parent, global::Gherkin.GherkinDialect dialect)
        {
            var steps = new List<Step>();
            StepX previous = new StepX(dialect.GivenStepKeywords.Where(g => g.Trim() != "*").First());
            foreach (var step in parent.Steps)
            {
                var name = previous.Name ?? dialect.GivenStepKeywords.Where(g => g.Trim() != "*").First();
                if (dialect.WhenStepKeywords.Contains(step.Keyword) || dialect.ThenStepKeywords.Contains(step.Keyword))
                {
                    name = step.Keyword;
                }
                steps.Add(previous = new StepX(step, name));
            }
            return steps.ToArray();
        }

        private IHasChildren UpdateSteps(IHasChildren parent, GherkinDialect dialect)
        {
            var newChilds = new List<IHasLocation>();
            foreach (var top in parent.Children)
            {
                if (top is IHasChildren)
                {
                    newChilds.Add(UpdateSteps((IHasChildren)top, dialect) as IHasLocation);
                }
                else if (top is IHasSteps)
                {
                    var newSteps = UpdateStepsX((IHasSteps)top, dialect);
                    newChilds.Add(top.ReplaceSteps(newSteps));
                }
            }

            if (parent is Rule)
            {
                newChilds = RuleElements(dialect, (Rule)parent).Union(newChilds).ToList();
            }

            return parent.ReplaceChildren(newChilds);
        }

        private GherkinDocument ReparseDocument(GherkinDocument doc)
        {
            return new GherkinDocument(UpdateSteps(doc.Feature, GetDialect(doc)) as Feature, doc.Comments.ToArray());
        }

        private static string CleanDescription(string desc)
        {
            while (desc.IndexOf("  ") > -1) desc = desc.Replace("  ", " ").Trim();
            return desc.Replace("\r\n ", "\r\n").Trim();
        }

        private static IEnumerable<IHasLocation> RuleElements(GherkinDialect dialect, Rule rule)
        {
            var desc = CleanDescription(rule.Description);
            var lineNumber = rule.Location.Line;

            var hasOneRule = false;
            foreach (var meta in dialect.MetaKeywords)
            {
                if (desc.StartsWith(meta.Trim()))
                {
                    lineNumber++;
                    foreach (Match match in Regex.Matches(desc, @"\*{2}(.*?)\*{2}", RegexOptions.Singleline))
                    {
                        var comment = match.Groups[0].Value.Trim().TrimStart(new char[] { '*' }).TrimEnd(new char[] { '*' }).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in comment)
                        {
                            if (line.IndexOf(":") > -1)
                            {
                                yield return new RuleMeta(
                                    new Location(++lineNumber, 0),
                                    line.Substring(0, line.IndexOf(":")).Trim(),
                                    line.Substring(line.IndexOf(":") + 1).Trim());
                                hasOneRule = true;
                            }
                        }
                        desc = CleanDescription(desc.Replace(match.Groups[0].Value, string.Empty));
                    }
                }
            }
            if (hasOneRule) lineNumber = lineNumber + 2;

            var lines = desc.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var current = 0;

            var hasTopLevel = false;

            IEnumerable<NarrativeMap> mapping;

            mapping = dialect.WhyKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeWhy) });
            foreach (var map in mapping)
            {
                if (current >= lines.Length) yield break;
                var gherkinLine = new GherkinLine(lines[current], current);
                if (gherkinLine.StartsWith(map.Key))
                {
                    yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length).Trim(), lineNumber + current);
                    current++;
                    hasTopLevel = true;
                    break;
                }
            }

            if (hasTopLevel)
            {
                hasTopLevel = false;
                // process And , But
                mapping = dialect.AndStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeAndWhy) })
                    .Union(dialect.ButStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeButWhy) }));

                var gotOne = false;
                while (true)
                {
                    foreach (var map in mapping)
                    {
                        if (current >= lines.Length) yield break;
                        var gherkinLine = new GherkinLine(lines[current], current);
                        if (gherkinLine.StartsWith(map.Key))
                        {
                            yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                            current++;
                            gotOne = true;
                            break;
                        }
                    }
                    if (!gotOne) break;
                    gotOne = false;
                }
            }

            mapping = dialect.WhoKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeWho) });
            foreach (var map in mapping)
            {
                if (current >= lines.Length) yield break;
                var gherkinLine = new GherkinLine(lines[current], current);
                if (gherkinLine.StartsWith(map.Key))
                {
                    yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                    current++;
                    hasTopLevel = true;
                    break;
                }
            }

            if (hasTopLevel)
            {
                hasTopLevel = false;

                // process And , But
                mapping = dialect.AndStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeAndWho) })
                    .Union(dialect.ButStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeButWho) }));

                var gotOne = false;
                while (true)
                {
                    foreach (var map in mapping)
                    {
                        if (current >= lines.Length) yield break;
                        var gherkinLine = new GherkinLine(lines[current], current);
                        if (gherkinLine.StartsWith(map.Key))
                        {
                            yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                            current++;
                            gotOne = true;
                            break;
                        }
                    }
                    if (!gotOne) break;
                    gotOne = false;
                }
            }

            mapping = dialect.WhereKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeWhere) });
            foreach (var map in mapping)
            {
                if (current >= lines.Length) yield break;
                var gherkinLine = new GherkinLine(lines[current], current);
                if (gherkinLine.StartsWith(map.Key))
                {
                    yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                    current++;
                    hasTopLevel = true;
                    break;
                }
            }

            if (hasTopLevel)
            {
                hasTopLevel = false;

                // process And , But
                mapping = dialect.AndStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeAndWhere) })
                    .Union(dialect.ButStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeButWhere) }));

                var gotOne = false;
                while (true)
                {
                    foreach (var map in mapping)
                    {
                        if (current >= lines.Length) yield break;
                        var gherkinLine = new GherkinLine(lines[current], current);
                        if (gherkinLine.StartsWith(map.Key))
                        {
                            yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                            current++;
                            gotOne = true;
                            break;
                        }
                    }
                    if (!gotOne) break;
                    gotOne = false;
                }
            }

            mapping = dialect.WhatKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeWhat) });
            foreach (var map in mapping)
            {
                if (current >= lines.Length) yield break;
                var gherkinLine = new GherkinLine(lines[current], current);
                if (gherkinLine.StartsWith(map.Key))
                {
                    yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                    current++;
                    hasTopLevel = true;
                    break;
                }
            }

            if (hasTopLevel)
            {
                hasTopLevel = false;

                // process And , But
                mapping = dialect.AndStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeAndWhat) })
                    .Union(dialect.ButStepKeywords.Select(w => new NarrativeMap { Key = w, Type = typeof(NarrativeButWhat) }));

                foreach (var map in mapping)
                {
                    if (current >= lines.Length) yield break;
                    var gherkinLine = new GherkinLine(lines[current], current);
                    if (gherkinLine.StartsWith(map.Key))
                    {
                        yield return CreateNarrative(map, gherkinLine.GetRestTrimmed(map.Key.Length), lineNumber + current);
                        current++;
                        break;
                    }
                }
            }
        }

        private struct NarrativeMap
        {
            public string Key;
            public Type Type;
        }

        private static INarrative CreateNarrative(NarrativeMap map, string description, int lineNumber)
        {
            var narrative = (INarrative)Activator.CreateInstance(map.Type, new object[] { });
            narrative.Name = map.Key.TrimEnd();
            narrative.Description = description;
            narrative.Location = new Location(lineNumber, 0);
            return narrative;
        }
    }
}