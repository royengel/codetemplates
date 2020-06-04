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

            if (string.IsNullOrEmpty(script))
                return c;

            int at = 0;
            string createTable = script.GetBetween("create table ", "(", ref at);
            if (string.IsNullOrEmpty(createTable))
                return c;

            c.Name = CleanName(createTable);
            if (c.Name.IndexOfAny(" \"[]".ToCharArray()) >= 0)
                c.Name = null;

            at--;

            string columns = script.GetBetween("(", ")", ref at);
            if (string.IsNullOrEmpty(columns))
                return c;

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
            if (property.Name.ToLower() == "agrtid")
                return null;

            string type = tokens[1];
            string typeArgument = "";
            int p = type.IndexOf("(");
            if (p >= 0)
            {
                int x = p;
                typeArgument = type.GetBetween("(", ")", ref x);
                type = type.Substring(0, p).SmartTrim();
            }
            else
            {
                type = type.SmartTrim();
            }

            property.Type = TryParseType(type, typeArgument, out int length, out int precision);

            if (string.IsNullOrEmpty(property.Type))
                return null;

            property.Length = length;
            property.Precision = precision;

            property.Nullable = column.ToLower().IndexOf("not null") == -1;

            return property;
        }

        protected static void ParseLengthAndPrecision(string arguments, out int length, out int precision)
        {
            int[] args = arguments.ToLower().Replace(" char", "")
                .Split(',').Select(s =>
            {
                int.TryParse(s, out int n);
                return n;
            }).ToArray();
            if (args.Length >= 1)
                length = args[0];
            else
                length = 0;
            if (args.Length >= 2)
                precision = args[1];
            else
                precision = 0;
        }

        internal abstract string TryParseType(string type, string arguments, out int length, out int precision);
        internal abstract string CleanName(string name);
    }
}
