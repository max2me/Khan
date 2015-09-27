using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Khan.Tests
{
	[TestFixture]
	public class UniteTests
	{
		[Test]
		public void Test()
		{
			var unite = new Unite();
			var source = @"<html><body>
<style type=""text/css-x"">
	.red { color: red; }
	.blue { color: blue; }
	table.slick { border-collapse: collapse; +border: 0; +cellpadding: 0; +cellspacing: 0; }
</style>
<p class=""red"">Should be red</p>
<p class=""blue"">Should be blue</p>
<table class=""slick"">
<tr><td>Lalala</td></tr>
</table>
</body></html>";

			var result = unite.Process(source);
			//result.ShouldBeEquivalentTo("asda");
		}
	}
}
