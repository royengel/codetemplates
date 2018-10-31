using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TailorTools.Props.Helpers;
using System.Linq;

namespace TailorTools.PropsTests.Helpers
{
    public class StringHelperTests
    {
        [Theory]
        [InlineData("a{{b}}c", 3, null, 3)]
        [InlineData("a{{b}}c", 2, "b", 5)]
        [InlineData("a{{b}}c", 0, "{b}", 6)]
        [InlineData("{{{b}}}c", 0, "{{b}}", 7)]
        [InlineData("{}", 0, "", 2)]
        public void WhenGetBetween_NestedParenthesisAreIgnored(string input, int at, string output, int expectedAt)
        {
            string result = input.GetBetween("{", "}", ref at);
            Assert.Equal(output, result);
            Assert.Equal(expectedAt, at);
        }

        [Theory]
        [InlineData("a(,),c", "a(,)", "c")]
        [InlineData("a(1,3),", "a(1,3)", "")]
        public void WhenSmartSplit_CommasInParenthesisesAreIgnored(string input, string part1, string part2)
        {
            var result = input.SmartSplit(",").ToArray();
            Assert.Equal(part1, result[0]);
            Assert.Equal(part2, result[1]);
        }
        [Theory]
        [InlineData("a", "a")]
        [InlineData("[a] ", "a")]
        [InlineData(@" ""a""", "a")]
        public void WhenSmartTrim_QuatesAndParenthesisesAreRemoved(string input, string output)
        {
            var result = input.SmartTrim();
            Assert.Equal(output, result);
        }
    }
}
