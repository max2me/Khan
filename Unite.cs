using CsQuery;

namespace Khan
{
    public class Unite
    {
        public string Process(string source)
        {
            var doc = CQ.Create(source);

            foreach (var astyle in doc.Select("style[type=text/css-x]"))
            {
                var cssSource = astyle.InnerText;
                astyle.Remove();

                foreach (var cssRule in CssParser.SplitIntoIndividualRules(cssSource))
                {
                    ApplyRuleInline(doc, cssRule);
                }
            }

            return doc.Render(DomRenderingOptions.RemoveComments | DomRenderingOptions.QuoteAllAttributes);
        }

        private void ApplyRuleInline(CQ doc, CssRule rule)
        {
            foreach (var selector in rule.Selectors)
            {
                // HACK: Since Razor by default encodes HTML
                var fixedSelector = selector.Replace("&gt;", ">");

                var element = doc.Select(fixedSelector);

                foreach (var cssKey in rule.CssProperties.AllKeys)
                    element.Css(cssKey, rule.CssProperties[cssKey]);

                foreach (var attrKey in rule.HtmlAttributes.AllKeys)
                    element.Attr(attrKey, rule.HtmlAttributes[attrKey]);
            }
        }
    }
}