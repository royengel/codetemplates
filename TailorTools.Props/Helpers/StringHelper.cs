using System;
using System.Collections.Generic;
using System.Text;

namespace TailorTools.Props.Helpers
{
    internal static class StringHelper
    {
        internal static string GetBetween(this string input, string fromToken, string toToken, ref int startAt)
        {
            int from = input.IndexOf(fromToken, startAt, StringComparison.InvariantCultureIgnoreCase);
            if (from < 0)
                return null;

            from += fromToken.Length;

            int to = input.IndexOf(toToken, from, StringComparison.InvariantCultureIgnoreCase);
            if (to < from)
                return null;

            return input.Substring(from, to - from);
        }

    }
}
