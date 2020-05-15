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

            return (type.ToLower()) switch
            {
                "bigint" => "long",
                "int" => "int",
                "smallint" => "short",
                "tinyint" => "byte",
                "bit" => "bool",
                "nvarchar" => "string",
                "varchar" => "string",
                "nchar" => "string",
                "char" => "string",
                "varbinary" => "byte[]",
                "uniqueidentifier" => "Guid",
                "datetime" => "DateTime",
                "datetime2" => "DateTime",
                "decimal" => "decimal",
                "numeric" => "decimal",
                "timestamp" => "byte[]",
                "rowversion" => "byte[]",
                _ => null,
            };
        }
    }
}
