using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TailorTools.Props.Helpers;
using TailorTools.Props.Models;

namespace TailorTools.Props.Parsers
{
    internal class ClassParser
    {
        internal Class ClassFromScript(string script)
        {
            Class c = new Class();

            int at = 0;
            string className = script.GetBetween("class ", "{", ref at);
            if (string.IsNullOrWhiteSpace(className) || className.Trim().Contains(" "))
                return c;

            c.Name = className.Trim();

            return c;
        }
    }
}
