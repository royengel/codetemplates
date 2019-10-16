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
                .Split('.')
                .Select(p => p.Trim()).Last();
        }

        internal override string TryParseType(string type, string arguments, out int length, out int precision)
        {
            if (arguments.Trim().ToLower() == "max")
            {
                length = -1;
                precision = 0;
            }
            else
            {
                ParseLengthAndPrecision(arguments, out length, out precision);
            }

            switch (type.ToLower())
            {
                case "bigint":
                    return "long";
                case "int":
                    return "int";
                case "smallint":
                    return "short";
                case "tinyint":
                    return "byte";
                case "bit":
                    return "bool";
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                case "nchar":
                    return "string";
                case "char":
                    return "string";
                case "varbinary":
                    return "byte[]";
                case "uniqueidentifier":
                    return "Guid";
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "decimal";
                case "numeric":
                    return "decimal";
            }
            return null;
        }
    }
}
