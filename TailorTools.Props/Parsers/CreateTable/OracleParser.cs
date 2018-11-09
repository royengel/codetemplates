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

        internal override string TryParseType(string type, string arguments, out int length, out int precision)
        {
            ParseLengthAndPrecision(arguments, out length, out precision);
            switch (type.ToLower())
            {
                case "number":
                    return TryParseNumber(arguments, length, precision);
                case "varchar2":
                    return "string";
                case "char2":
                    return "string";
                case "clob":
                    length = -1;
                    return "string";
                case "blob":
                    length = -1;
                    return "byte[]";
                case "raw":
                    if(length == 16)
                        return "Guid";
                    return "byte[]";
                case "date":
                    return "DateTime";
            }
            return null;
        }

        private string TryParseNumber(string arguments, int length, int precision)
        {
            if (precision == 0)
            {
                if (length > 15)
                    return "long";
                if (length > 5)
                    return "int";
                if (length > 3)
                    return "short";
                if (length > 1)
                    return "byte";
                if (length > 0)
                    return "bool";
            }
            else
            {
                return "decimal";
            }
            return null;
        }
    }
}
