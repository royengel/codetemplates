using System;
using System.Linq;
using TailorTools.Props;
using TailorTools.Props.Models;
using Xunit;

namespace TailorTools.PropsTests
{
    public class ConverterTests
    {
        private Converter Converter;
        public ConverterTests()
        {
            Converter = new Converter();
        }

        [Theory]
        [InlineData(@"CREATE TABLE ""AGRM7"".""table1"" (""column1"" NUMBER(15, 0) DEFAULT 0 NOT NULL ENABLE);")]
        [InlineData(@"CREATE TABLE [dbo].[table1]([column1] [int] NOT NULL)")]
        [InlineData(@"public class table1 { public int column1 { get; set; } }")]
        [InlineData(@"create table table1(")]
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
            Assert.Equal("int", property.Type);
            Assert.False(property.Nullable);
        }

        [Theory]
        [InlineData("[bigint]", "long", 0, 0, true)]
        [InlineData("[int] NULL", "int", 0, 0, true)]
        [InlineData("[smallint] NOT NULL", "short", 0, 0, false)]
        [InlineData("[tinyint] NOT NULL", "byte", 0, 0, false)]
        [InlineData("[bit] NOT NULL", "bool", 0, 0, false)]
        [InlineData("[nvarchar](25)", "string", 25, 0, true)]
        [InlineData("[nvarchar](max)", "string", -1, 0, true)]
        [InlineData("[varchar](25)", "string", 25, 0, true)]
        [InlineData("[nchar](25)", "string", 25, 0, true)]
        [InlineData("[char](25)", "string", 25, 0, true)]
        [InlineData("[varbinary](max)", "byte[]", -1, 0, true)]
        [InlineData("[uniqueidentifier]", "Guid", 0, 0, true)]
        [InlineData("[decimal](28, 3)", "decimal", 28, 3, true)]
        [InlineData("[numeric](28, 3)", "decimal", 28, 3, true)]
        [InlineData("[datetime]", "DateTime", 0, 0, true)]
        public void WhenParse_MsSqlGetsColumnsRight(string type, string expectedType, int length, int precision, bool nullable)
        {
            TestProperty($@"CREATE TABLE [dbo].[table1]([column1] {type})", expectedType, length, precision, nullable);
        }

        [Theory]
        [InlineData("NUMBER(20,0) DEFAULT 0 NOT NULL ENABLE", "long", 20, 0, false)]
        [InlineData("NUMBER(15,0) NULL", "int", 15, 0, true)]
        [InlineData("NUMBER(5,0)", "short", 5, 0, true)]
        [InlineData("NUMBER(3,0) NOT NULL", "byte", 3, 0, false)]
        [InlineData("NUMBER(1,0) NOT NULL", "bool", 1, 0, false)]
        [InlineData("VARCHAR2(25 CHAR)", "string", 25, 0, true)]
        [InlineData("VARCHAR2(25)", "string", 25, 0, true)]
        [InlineData("CLOB", "string", -1, 0, true)]
        [InlineData("BLOB", "byte[]", -1, 0, true)]
        [InlineData("RAW(16)", "Guid", 16, 0, true)]
        [InlineData("NUMBER(30,3)", "decimal", 30, 3, true)]
        [InlineData("DATE", "DateTime", 0, 0, true)]
        public void WhenParse_OracleGetsColumnsRight(string type, string expectedType, int length, int precision, bool nullable)
        {
            TestProperty($@"CREATE TABLE ""AGRM7"".""table1"" (""column1"" {type});", expectedType, length, precision, nullable);
        }

        [Theory]
        [InlineData("Int64", "long", 0, 0, false)]
        [InlineData("Int32", "int", 0, 0, false)]
        [InlineData("Int16", "short", 0, 0, false)]
        [InlineData("byte", "byte", 0, 0, false)]
        [InlineData("Boolean", "bool", 0, 0, false)]
        [InlineData("String", "string", 25, 0, false)]
        [InlineData("byte[]", "byte[]", -1, 0, false)]
        [InlineData("Guid", "Guid", 0, 0, false)]
        [InlineData("Decimal", "decimal", 28, 3, false)]
        [InlineData("DateTime", "DateTime", 0, 0, false)]
        public void WhenParse_ClassGetsColumnsRight(string type, string expectedType, int length, int precision, bool nullable)
        {
            TestProperty("public class table1 { public " + type + " column1 { get; set; } }", expectedType, length, precision, nullable);
        }


        private void TestProperty(string script, string expectedType, int length, int precision, bool nullable)
        {
            Class c = Converter.ClassFromScript(script);
            Property property = c.Properties.Single();
            Assert.Equal(expectedType, property.Type);
            Assert.Equal(length, property.Length);
            Assert.Equal(precision, property.Precision);
            Assert.Equal(nullable, property.Nullable);
        }

        [Fact]
        public void WhenParseMsSqlCrap_OnlyRelevantColumns()
        {
            TestProperty(@"CREATE TABLE [dbo].[agltransact](
	[voucher_type] [nvarchar](25) NOT NULL,
	[agrtid] [bigint] IDENTITY(1,1) NOT NULL,
UNIQUE NONCLUSTERED 
(
	[agrtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]", "string", 25, 0, false);
        }
    }
}
