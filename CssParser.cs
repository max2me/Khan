using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Khan
{
    public class CssParser
    {
        public static IEnumerable<CssRule> SplitIntoIndividualRules(string cssSource)
        {
            cssSource = RemoveComments(cssSource).Replace("&amp;", "&");

            var rules = new List<CssRule>();

            foreach (var ruleCandidate in cssSource.Split('}'))
            {
                if (String.IsNullOrWhiteSpace(ruleCandidate))
                    continue;

                NameValueCollection props, attrs;

                ParseProperties(ruleCandidate, out props, out attrs);

                var cssRule = new CssRule {Selectors = ParseSelectors(ruleCandidate), CssProperties = props, HtmlAttributes = attrs};

                if (cssRule.Selectors.Count > 0)
                    rules.Add(cssRule);
            }

            return rules;
        }

        private static string RemoveComments(string cssSource)
        {
            return Regex.Replace(cssSource, @"/\*.+?\*/", string.Empty, RegexOptions.Singleline);
        }

        public static void ParseProperties(string rule, out NameValueCollection cssProperties, out NameValueCollection htmlAttributes)
        {
            rule = RemoveComments(rule);

            var propertiesString = rule.Substring(rule.IndexOf("{") + 1);
            cssProperties = new NameValueCollection();
            htmlAttributes = new NameValueCollection();


            // HACK: &#39; needed because Razor encodes everything
            foreach (var propString in propertiesString.Replace("&#39;", "'").Split(';'))
            {
                if (String.IsNullOrWhiteSpace(propString))
                    continue;

                var colonPos = propString.IndexOf(':');
                var name = propString.Substring(0, colonPos).Trim();
                var value = propString.Substring(colonPos + 1).Trim();

                if (name.StartsWith("+"))
                    htmlAttributes.Add(name.Substring(1), value);
                else
                    cssProperties.Add(name, value);
            }
        }

        public static StringCollection ParseSelectors(string ruleCandidate)
        {
            ruleCandidate = RemoveComments(ruleCandidate);

            var selectors = new StringCollection();

            if (String.IsNullOrWhiteSpace(ruleCandidate))
                return selectors;

            var selectorsString = ruleCandidate.Substring(0, ruleCandidate.IndexOf("{"));

            foreach (var s in selectorsString.Split(','))
            {
                var selector = s.Trim();
                selectors.Add(selector);
            }

            return selectors;
        }
    }
}