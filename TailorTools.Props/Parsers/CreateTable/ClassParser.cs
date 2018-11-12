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
            at--;

            string classDef = script.GetBetween("{", "}", ref at);
            if (string.IsNullOrEmpty(classDef))
                return c;

            List<Property> properties = new List<Property>();
            foreach (var propDef in classDef.SmartSplit("\n"))
            {
                Property property = parseProperty(propDef);
                if (property != null)
                    properties.Add(property);
            }

            c.Properties = properties.ToArray();

            return c;
        }

        private Property parseProperty(string propDef)
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
            switch (type)
            {
                case "string":
                    return 25;
                case "byte[]":
                    return -1;
                case "decimal":
                    return 28;
            }
            return 0;
        }

        private int PrecisionFromType(string type)
        {
            switch (type)
            {
                case "decimal":
                    return 3;
            }
            return 0;
        }

        private string Istype(string type)
        {
            switch (type)
            {
                case "long":
                    return "long";
                case "Int64":
                    return "long";
                case "int":
                    return "int";
                case "Int32":
                    return "int";
                case "short":
                    return "short";
                case "Int16":
                    return "short";
                case "byte":
                    return "byte";
                case "bool":
                    return "bool";
                case "Boolean":
                    return "bool";
                case "string":
                    return "string";
                case "String":
                    return "string";
                case "byte[]":
                    return "byte[]";
                case "Guid":
                    return "Guid";
                case "DateTime":
                    return "DateTime";
                case "decimal":
                    return "decimal";
                case "Decimal":
                    return "decimal";
            }
            return null;
        }

    }
}
