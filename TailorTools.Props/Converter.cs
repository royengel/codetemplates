using System.Collections.Generic;
using System.Linq;
using TailorTools.Props.Models;
using TailorTools.Props.Parsers;

namespace TailorTools.Props
{
    public class Converter
    {
        public static Class ClassFromScript(string script)
        {
            var results = new List<Class>();
            MsSqlParser msSqlParser = new MsSqlParser();
            results.Add(msSqlParser.ClassFromScript(script));

            OracleParser oracleParser = new OracleParser();
            results.Add(oracleParser.ClassFromScript(script));

            ClassParser classParser = new ClassParser();
            results.Add(classParser.ClassFromScript(script));

            return results
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .OrderByDescending(c => c.Properties?.Count())
                .FirstOrDefault();
        }
    }
}
