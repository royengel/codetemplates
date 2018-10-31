using System;
using System.Collections.Generic;
using System.Text;

namespace TailorTools.Props.Helpers
{
    public static class StringHelper
    {
        private const string _startParenthesis = "{[('\"";
        private const string _endParenthesis = "}])'\"";

        public static string GetBetween(this string input, string fromToken, string toToken, ref int startAt)
        {
            int from = input.IndexOf(fromToken, startAt, StringComparison.InvariantCultureIgnoreCase);
            if (from < 0)
                return null;

            from += fromToken.Length;
            int to;

            to = NextToken(input, toToken, from);

            if (to < from)
                return null;

            startAt = to + toToken.Length;

            return input.Substring(from, to - from);
        }

        private static int NextToken(string input, string token, int from)
        {
            int any = input.IndexOfAny("{[('\"".ToCharArray(), from);
            int to = input.IndexOf(token, from, StringComparison.InvariantCultureIgnoreCase);
            while (to > from && any >= from && any < to)
            {
                int skip = any;
                string sub = Match(input, input[any], ref skip);
                any = input.IndexOfAny("{[('\"".ToCharArray(), skip);
                to = input.IndexOf(token, skip, StringComparison.InvariantCultureIgnoreCase);
            }

            return to;
        }

        private static string Match(string input, char fromToken, ref int jump)
        {
            char toToken = _endParenthesis[_startParenthesis.IndexOf(fromToken)];
            return input.GetBetween(fromToken.ToString(), toToken.ToString(), ref jump);
        }

        public static IEnumerable<string> SmartSplit(this string input, string token)
        {
            int from = 0;
            int to = 0;
            while ((to = NextToken(input, token, from)) >= 0)
            {
                yield return input.Substring(from, to - from);
                from = to + token.Length;
            }
            yield return input.Substring(from);
        }
        public static string SmartTrim(this string input)
        {
            input = input.Trim();
            int s = input.IndexOfAny(_startParenthesis.ToCharArray());
            if(s == 0)
            {
                char endToken = _endParenthesis[_startParenthesis.IndexOf(input[0])];
                if (input[input.Length - 1] == endToken)
                    input = input.Substring(1, input.Length - 2).Trim();
            }
            return input;
        }
    }
}
