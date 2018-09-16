using System;
using System.Linq;
using Thumnet.CoverageReport.Core.Parsers;
using Xunit;

namespace Thumnet.CoverageReport.Test.Core.Parsers
{
    public class LcovParserTest
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException()
        {
            // Arrange
            string text = null;
            Exception expectedException = null;

            // Act
            try
            {
                var parser = new LcovParser(text);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            // Assert
            Assert.NotNull(expectedException);
            Assert.IsType<ArgumentNullException>(expectedException);
        }

        [Fact]
        public void ReadItems_Returns_EmptyList()
        {
            // Arrange
            string text = "test";
            var parser = new LcovParser(text);

            // Act
            var items = parser.ReadItems();

            // Assert
            Assert.Empty(items);
        }

        [Fact]
        public void ReadItems_Returns_OneItem()
        {
            // Arrange
            string text = @"SF:src\CoverageTest\CoverageTest.App\Calculator.cs
FN:9,System.Int32 CoverageTest.App.Calculator::Add(System.Int32,System.Int32)
FNDA:1,System.Int32 CoverageTest.App.Calculator::Add(System.Int32,System.Int32)
DA:10,1
DA:11,1
DA:12,1
LF:3
LH:3
BRF:0
BRH:0
FNF:1
FNH:1
end_of_record";

            var parser = new LcovParser(text);

            // Act
            var items = parser.ReadItems().ToList();

            // Assert
            Assert.Single(items);
            var item = items[0];


            Assert.Equal(@"src\CoverageTest\CoverageTest.App\Calculator.cs", item.File);

            Assert.Single(item.Functions.Details);
            Assert.Equal(1, item.Functions.Found);
            Assert.Equal(1, item.Functions.Hit);
            Assert.Equal("System.Int32 CoverageTest.App.Calculator::Add(System.Int32,System.Int32)", item.Functions.Details[0].Name);
            Assert.Equal(9, item.Functions.Details[0].Line);
            Assert.Equal(1, item.Functions.Details[0].Hit);

            Assert.Empty(item.Branches.Details);
            Assert.Equal(0, item.Branches.Found);
            Assert.Equal(0, item.Branches.Hit);

            Assert.Equal(3, item.Lines.Details.Count);
            Assert.All(item.Lines.Details, d => Assert.Equal(1, d.Hit));
            Assert.Equal(10, item.Lines.Details[0].Line);
            Assert.Equal(11, item.Lines.Details[1].Line);
            Assert.Equal(12, item.Lines.Details[2].Line);
        }
    }
}
