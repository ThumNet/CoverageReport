using System.Text;
using Thumnet.CoverageReport.Core.Helpers;
using Xunit;

namespace Thumnet.CoverageReport.Test.Core.Helpers
{
    public class Md5HashTest
    {
        [Fact]
        public void Create_Handles_Null()
        {
            // Arrange
            byte[] data = null;

            // Act
            var hash = Md5Hash.Create(data);

            // Assert
            Assert.Null(hash);
        }

        [Fact]
        public void Create_Handles_EmptyArray()
        {
            // Arrange
            byte[] data = new byte[] { };

            // Act
            var hash = Md5Hash.Create(data);

            // Assert
            Assert.Null(hash);
        }

        [Fact]
        public void Create_Handles_TextBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("test");

            // Act
            var hash = Md5Hash.Create(data);

            // Assert
            Assert.Equal("098F6BCD4621D373CADE4E832627B4F6", hash);
        }
    }
}
