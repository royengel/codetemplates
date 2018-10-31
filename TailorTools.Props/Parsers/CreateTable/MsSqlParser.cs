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
                    return typeof(long).ToString();
                case "int":
                    return typeof(int).ToString();
                case "smallint":
                    return typeof(short).ToString();
                case "tinyint":
                    return typeof(byte).ToString();
                case "bit":
                    return typeof(bool).ToString();
                case "nvarchar":
                    return typeof(string).ToString();
                case "varchar":
                    return typeof(string).ToString();
                case "nchar":
                    return typeof(string).ToString();
                case "char":
                    return typeof(string).ToString();
                case "varbinary":
                    return typeof(byte[]).ToString();
                case "uniqueidentifier":
                    return typeof(Guid).ToString();
                case "datetime":
                    return typeof(DateTime).ToString();
                case "decimal":
                    return typeof(decimal).ToString();
            }
            return null;
        }
    }
}
