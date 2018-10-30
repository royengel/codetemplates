using System;
using System.Linq;
using TailorTools.Props;
using TailorTools.Props.Models;
using Xunit;

namespace TailorTools.PropsTests
{
    public class ConverterTests
    {
        [Theory]
        [InlineData(@"CREATE TABLE ""AGRM7"".""table1"" (""column1"" NUMBER(15, 0) DEFAULT 0 NOT NULL ENABLE);")]
        [InlineData(@"CREATE TABLE [dbo].[table1]([column1] [int] NOT NULL)")]
        [InlineData(@"public class table1 { public int column1 { get; set; } }")]
        public void WhenParse_ClassNameFound(string script)
        {
            Class c = Converter.ClassFromScript(script);
            Assert.Equal("table1", c.Name);
        }

        [Theory]
        [InlineData(@"CREATE TABLE ""AGRM7"".""table1"" (""column1"" NUMBER(15, 0) DEFAULT 0 NOT NULL ENABLE);")]
        [InlineData(@"CREATE TABLE [dbo].[table1]([column1] [int] NOT NULL)")]
        [InlineData(@"public class table1 { public int column1 { get; set; } }")]
        public void WhenInt_IntPropertyGotParsed(string script)
        {
            Class c = Converter.ClassFromScript(script);
            Property property = c.Properties.Single();
            Assert.Equal("column1", property.Name);
            Assert.Equal("System.Int32", property.Type);
            Assert.False(property.Nullable);
        }
    }
}
