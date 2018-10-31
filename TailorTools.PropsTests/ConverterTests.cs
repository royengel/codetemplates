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

        [Theory]
        [InlineData("[bigint]", "System.Int64", 0, 0, true)]
        [InlineData("[int] NULL", "System.Int32", 0, 0, true)]
        [InlineData("[smallint] NOT NULL", "System.Int16", 0, 0, false)]
        [InlineData("[tinyint] NOT NULL", "System.Byte", 0, 0, false)]
        [InlineData("[bit] NOT NULL", "System.Boolean", 0, 0, false)]
        [InlineData("[nvarchar](25)", "System.String", 25, 0, true)]
        [InlineData("[nvarchar](max)", "System.String", -1, 0, true)]
        [InlineData("[varchar](25)", "System.String", 25, 0, true)]
        [InlineData("[nchar](25)", "System.String", 25, 0, true)]
        [InlineData("[char](25)", "System.String", 25, 0, true)]
        [InlineData("[varbinary](max)", "System.Byte[]", -1, 0, true)]
        [InlineData("[uniqueidentifier]", "System.Guid", 0, 0, true)]
        [InlineData("[decimal](28, 3)", "System.Decimal", 28, 3, true)]
        [InlineData("[datetime]", "System.DateTime", 0, 0, true)]
        public void WhenParse_MsSqlGetsColumnsRight(string type, string expectedType, int length, int precision, bool nullable)
        {
            TestProperty($@"CREATE TABLE [dbo].[table1]([column1] {type})", expectedType, length, precision, nullable);
        }

        [Theory]
        [InlineData("NUMBER(20,0) DEFAULT 0 NOT NULL ENABLE", "System.Int64", 20, 0, false)]
        [InlineData("NUMBER(15,0) NULL", "System.Int32", 15, 0, true)]
        [InlineData("NUMBER(5,0)", "System.Int16", 5, 0, true)]
        [InlineData("NUMBER(3,0) NOT NULL", "System.Byte", 3, 0, false)]
        [InlineData("NUMBER(1,0) NOT NULL", "System.Boolean", 1, 0, false)]
        [InlineData("VARCHAR2(25 CHAR)", "System.String", 25, 0, true)]
        [InlineData("VARCHAR2(25)", "System.String", 25, 0, true)]
        [InlineData("CLOB", "System.String", -1, 0, true)]
        [InlineData("BLOB", "System.Byte[]", -1, 0, true)]
        [InlineData("RAW(16)", "System.Guid", 16, 0, true)]
        [InlineData("NUMBER(30,3)", "System.Decimal", 30, 3, true)]
        [InlineData("DATE", "System.DateTime", 0, 0, true)]
        public void WhenParse_OracleGetsColumnsRight(string type, string expectedType, int length, int precision, bool nullable)
        {
            TestProperty($@"CREATE TABLE ""AGRM7"".""table1"" (""column1"" {type});", expectedType, length, precision, nullable);
        }

        private static void TestProperty(string script, string expectedType, int length, int precision, bool nullable)
        {
            Class c = Converter.ClassFromScript(script);
            Property property = c.Properties.Single();
            Assert.Equal(expectedType, property.Type);
            Assert.Equal(length, property.Length);
            Assert.Equal(precision, property.Precision);
            Assert.Equal(nullable, property.Nullable);
        }
    }
}
