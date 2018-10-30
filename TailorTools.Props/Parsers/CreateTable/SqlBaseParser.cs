using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TailorTools.Props.Helpers;
using TailorTools.Props.Models;

namespace TailorTools.Props.Parsers
{
    internal abstract class SqlBaseParser
    {
        internal Class ClassFromScript(string script)
        {
            Class c = new Class();

            int at = 0;
            string createTable = script.GetBetween("create table ", "(", ref at);
            if (string.IsNullOrEmpty(createTable))
                return c;

            c.Name = CleanName(createTable);
            if (c.Name.IndexOfAny(" \"[]".ToCharArray()) >= 0)
                c.Name = null;

            return c;
        }
        internal abstract string CleanName(string name);
    }
}
