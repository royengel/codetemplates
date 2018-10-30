using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TailorTools.Props.Helpers;
using TailorTools.Props.Models;

namespace TailorTools.Props.Parsers
{
    internal class MsSqlParser : SqlBaseParser
    {
        internal override string CleanName(string name)
        {
            return name
                .Replace("[", "")
                .Replace("]", "")
                .Split('.').Select(p => p.Trim()).Last();
        }

    }
}
