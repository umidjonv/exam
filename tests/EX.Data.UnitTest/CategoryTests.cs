using System.Linq;
using EX.Data.Core;
using EX.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EX.Data.UnitTest
{

    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        public void Get_All()
        {
            // Arrange
            var data = new[]
            {
                new Category { Name = "AAA" },
                new Category { Name = "BBB" },
                new Category { Name = "CCC" },
            }.AsQueryable();

            // Act 
            var mockSet = MockHelper.CreateDbSetMock(data);
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            // Assert
            var regions = mockContext.Object.Categories.ToArray();

            Assert.AreEqual(3, regions.Count());
            Assert.AreEqual("AAA", regions[0].Name);
            Assert.AreEqual("BBB", regions[1].Name);
            Assert.AreEqual("CCC", regions[2].Name);
        }

    }
}
