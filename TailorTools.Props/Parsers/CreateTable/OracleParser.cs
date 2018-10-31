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
                    return typeof(string).ToString();
                case "char2":
                    return typeof(string).ToString();
                case "clob":
                    length = -1;
                    return typeof(string).ToString();
                case "blob":
                    length = -1;
                    return typeof(byte[]).ToString();
                case "raw":
                    if(length == 16)
                        return typeof(Guid).ToString();
                    return typeof(byte[]).ToString();
                case "date":
                    return typeof(DateTime).ToString();
            }
            return null;
        }

        private string TryParseNumber(string arguments, int length, int precision)
        {
            if (precision == 0)
            {
                if (length > 15)
                    return typeof(long).ToString();
                if (length > 5)
                    return typeof(int).ToString();
                if (length > 3)
                    return typeof(short).ToString();
                if (length > 1)
                    return typeof(byte).ToString();
                if (length > 0)
                    return typeof(bool).ToString();
            }
            else
            {
                return typeof(decimal).ToString();
            }
            return null;
        }
    }
}
