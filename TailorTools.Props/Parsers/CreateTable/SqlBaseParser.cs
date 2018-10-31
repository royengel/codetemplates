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

            at--;
            string columns = script.GetBetween("(", ")", ref at);
            List<Property> properties = new List<Property>();
            foreach(var column in columns.SmartSplit(","))
            {
                Property property = TryParseColumn(column);
                if (property != null)
                    properties.Add(property);
            }

            c.Properties = properties.ToArray();

            return c;
        }

        protected Property TryParseColumn(string column)
        {
            var tokens = column.Trim().Replace(" (", "(").SmartSplit(" ").ToArray();
            if (tokens.Length < 2)
                return null;

            Property property = new Property();
            property.Name = tokens[0].SmartTrim();
            if (string.IsNullOrEmpty(property.Name))
                return null;

            string type = tokens[1];
            string typeArgument = "";
            int p = type.IndexOf("(");
            if (p >= 0)
            {
                typeArgument = type.GetBetween("(", ")", ref p);
                type = type.Substring(0, p).SmartTrim();
            }
            else
            {
                type = type.SmartTrim();
            }

            property.Type = TryParseType(type, typeArgument, out int length, out int precision);
            property.Length = length;
            property.Precision = precision;

            return property;
        }

        internal abstract string TryParseType(string type, string arguments, out int length, out int precision);
        internal abstract string CleanName(string name);
    }
}
