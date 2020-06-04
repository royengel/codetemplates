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

            if (string.IsNullOrWhiteSpace(script))
                return c;

            int at = 0;
            string className = script.GetBetween("class ", "{", ref at);
            if (string.IsNullOrWhiteSpace(className) || className.Trim().Contains(" "))
                return c;

            c.Name = className.Trim();
            at--;

            string classDef = script.GetBetween("{", "}", ref at);
            if (string.IsNullOrEmpty(classDef))
                return c;

            List<Property> properties = new List<Property>();
            foreach (var propDef in classDef.SmartSplit("\n"))
            {
                Property property = ParseProperty(propDef);
                if (property != null)
                    properties.Add(property);
            }

            c.Properties = properties.ToArray();

            return c;
        }

        private Property ParseProperty(string propDef)
        {
            var tokens = propDef.Trim().SmartSplit(" ").ToArray();
            if (tokens.Length < 2)
                return null;

            for(int i = 0; i < tokens.Length - 2; i++)
            {
                string type = Istype(tokens[i]);
                if(!string.IsNullOrEmpty(type) && tokens[i + 2].StartsWith("{"))
                {
                    return CreateProperty(type, tokens[i + 1].Trim());
                }
            }
            return null;
        }

        private Property CreateProperty(string type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            Property property = new Property()
            {
                Name = name.Trim(),
                Type = type
            };

            property.Nullable = false;
            property.Length = LengthFromType(type);
            property.Precision = PrecisionFromType(type);

            return property;
        }

        private int LengthFromType(string type)
        {
            return type switch
            {
                "string" => 25,
                "byte[]" => -1,
                "decimal" => 28,
                _ => 0,
            };
        }

        private int PrecisionFromType(string type)
        {
            return type switch
            {
                "decimal" => 3,
                _ => 0,
            };
        }

        private string Istype(string type)
        {
            return type switch
            {
                "long" => "long",
                "Int64" => "long",
                "int" => "int",
                "Int32" => "int",
                "short" => "short",
                "Int16" => "short",
                "byte" => "byte",
                "bool" => "bool",
                "Boolean" => "bool",
                "string" => "string",
                "String" => "string",
                "byte[]" => "byte[]",
                "Guid" => "Guid",
                "DateTime" => "DateTime",
                "decimal" => "decimal",
                "Decimal" => "decimal",
                _ => null,
            };
        }

    }
}
