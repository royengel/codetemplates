using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TailorTools.Props.Helpers;
using TailorTools.Props.Models;

namespace TailorTools.Props.Parsers
{
    internal class OracleParser : SqlBaseParser
    {
        internal override string CleanName(string name)
        {
            return name.Replace("\"", "")
                .Split('.').Select(p => p.Trim()).Last();
        }

    }
}
