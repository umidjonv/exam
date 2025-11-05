using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Web.Controllers;
using EX.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using X.PagedList;
using Xunit;
using Xunit.Abstractions;

namespace EX.Web.IntegrationTest
{
    public class HomeTests
    {
        private readonly ITestOutputHelper _output;

        public HomeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Render_Home_Page()
        {
            // Arrange
            var data = new[]
            {
                new Document {Title = "AAA"},
                new Document {Title = "BBB"},
                new Document {Title = "CCC"},
            }.AsQueryable();
            var mockSet = MockHelper.CreateDbSetMock(data);
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Documents).Returns(mockSet.Object);
            var controller = new HomeController(mockContext.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);

            var regions = mockContext.Object.Documents.ToArray();
            var docs = (IPagedList<Document>)controller.ViewBag.Docs;
            Assert.NotNull(docs);
            Assert.Equal(3, docs.Count);
            Assert.Same(docs.ElementAt(0).Title, regions.ElementAt(0).Title);
            Assert.Same(docs.ElementAt(1).Title, regions.ElementAt(1).Title);
            Assert.Same(docs.ElementAt(2).Title, regions.ElementAt(2).Title);

            _output.WriteLine(JsonConvert.SerializeObject(docs));
        }

        [Fact]
        public void Throw_Empty_Error()
        {
            // Arrange
            var mockDb = new Mock<IAppDbContext>();
            var controller = new HomeController(mockDb.Object);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);

            Assert.NotNull(model.Exception);
            Assert.Equal("", model.Exception.Message);
            Assert.Empty(model.Path);

            _output.WriteLine(JsonConvert.SerializeObject(model));
        }
    }
}
