using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Khan.Tests
{
	[TestFixture]
	public class CssParserTests
	{
		[Test]
		public void ParseProperties()
		{
			NameValueCollection css, html;
			const string rule = ".selector1, .selector2 { background-color: red; +border: 0; +cellpadding: 0; border: 1px solid black}";

			CssParser.ParseProperties(rule, out css, out html);

			css.ShouldBeEquivalentTo(new NameValueCollection
				{
					{"background-color", "red"},
					{"border", "1px solid black"}
				});

			html.ShouldBeEquivalentTo(new NameValueCollection
				{
					{"border", "0"},
					{"cellpadding", "0"}
				});
		}

		[Test]
		public void ParseSelectors()
		{
			StringCollection selectors = CssParser.ParseSelectors(".selector1, .selector2 { background-color: red; ");

			selectors.Should().BeEquivalentTo(new StringCollection
				{
					".selector1",
					".selector2"
				});
		}

		[Test]
		public void OverlyENcodedValues()
		{
			IEnumerable<CssRule> actual = CssParser.SplitIntoIndividualRules(".selector1, .selector2 { background-color: &amp;#39;red&amp;#39;; ");

			var expected = new List<CssRule>()
			{
				new CssRule
				{
					CssProperties = new NameValueCollection {{"background-color", "'red'"}},
					Selectors = new StringCollection {".selector1", ".selector2"},
					HtmlAttributes = new NameValueCollection()
				}
			};

			actual.First().ShouldBeEquivalentTo(expected.First());
		}


		[Test]
		public void ParseSelectorsWithComments()
		{
			StringCollection selectors = CssParser.ParseSelectors(".selector1, /* comment */ .selector2 { background-color: red; ");

			selectors.Should().BeEquivalentTo(new StringCollection
				{
					".selector1",
					".selector2"
				});
		}

		[Test]
		public void SplitIntoIndividualRules()
		{
			const string css = @".selector1, .selector2 { background-color: red; }
.selector1, #selector2 { background-color: red; +border: 0; +cellpadding: 0; border: 1px solid black}";

			IEnumerable<CssRule> actual = CssParser.SplitIntoIndividualRules(css);
			var expected = new List<CssRule>
				{
					new CssRule
						{
							CssProperties = new NameValueCollection {{"background-color", "red"}},
							Selectors = new StringCollection {".selector1", ".selector2"},
							HtmlAttributes = new NameValueCollection()
						},
					new CssRule
						{
							CssProperties = new NameValueCollection
								{
									{"background-color", "red"},
									{"border", "1px solid black"}
								},
							HtmlAttributes = new NameValueCollection
								{
									{"border", "0"},
									{"cellpadding", "0"}
								},
							Selectors = new StringCollection {".selector1", "#selector2"}
						}
				};

			actual.ShouldBeEquivalentTo(expected);
		}
	}
}