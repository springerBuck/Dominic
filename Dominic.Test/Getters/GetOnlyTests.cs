using System.IO;
using System.Threading.Tasks;
using Dominic.Exceptions;
using Xunit;

namespace Dominic.Test.Getters
{
    public class GetOnlyTests
    {
        private DominicConfiguration _configuration;

        public GetOnlyTests()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var viewPath = $"{currentDirectory}/Views/TestView";
            _configuration = new DominicConfiguration { ViewFolderLocation = viewPath };
        }

        [Fact]
        public async Task ById_ItCanGet()
        {
            var sut = await Template.Render(
                "MultipleUniqueIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Equal("My Div One, Hello World", sut.GetOnly.ById("div-1").InnerText);
        }

        [Fact]
        public async Task ById_ItThrowsAnErrorWhenMoreThanOneElementFound()
        {
            var sut = await Template.Render(
                "MultipleDuplicateIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Throws<TooManyElementsFoundException>(() => sut.GetOnly.ById("div-1"));
        }

        [Fact]
        public async Task ById_ItReturnsNullWhenNoElementFound()
        {
            var sut = await Template.Render(
                "MultipleUniqueIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Null(sut.GetOnly.ById("not-a-real-id"));
        }

        [Fact]
        public async Task ByTestId_ItCanGet()
        {
            var sut = await Template.Render(
                "Article.cshtml",
                _configuration,
                new { Title = "A cool title", Author = "Aaron Buckley" });
            Assert.Equal("A cool title", sut.GetOnly.ByTestId("first-h2").InnerText);
        }

        [Fact]
        public async Task ByTestId_ItThrowsAnErrorWhenMoreThanOneElementFound()
        {
            var sut = await Template.Render(
                "MultipleDuplicateIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Throws<TooManyElementsFoundException>(() => sut.GetOnly.ByTestId("my-test-id"));
        }

        [Fact]
        public async Task ByTestId_ItReturnsNullWhenNoElementFound()
        {
            var sut = await Template.Render(
                "MultipleUniqueIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Null(sut.GetOnly.ByTestId("not-a-real-id"));
        }

        [Fact]
        public async Task ByType_ItCanGet()
        {
            var sut = await Template.Render(
                "Article.cshtml",
                _configuration,
                new { Title = "A cool title", Author = "Aaron Buckley" });
            Assert.Equal("Now this is a story!", sut.GetOnly.ByType("h1").InnerText);
        }

        [Fact]
        public async Task ByType_ItThrowsAnErrorWhenMoreThanOneElementFound()
        {
            var sut = await Template.Render(
                "MultipleDuplicateIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Throws<TooManyElementsFoundException>(() => sut.GetOnly.ByType("div"));
        }

        [Fact]
        public async Task ByType_ItReturnsNullWhenNoElementFound()
        {
            var sut = await Template.Render(
                "MultipleUniqueIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Null(sut.GetOnly.ByType("not-real"));
        }

        [Fact]
        public async Task ByPartialName_ItCanGet()
        {
            var sut = await Template.Render(
                "Article.cshtml",
                _configuration,
                new { Title = "A cool title", Author = "Aaron Buckley" });
            Assert.NotNull(sut.GetOnly.ByPartialName("_PartialName.cshtml"));
        }

        [Fact]
        public async Task ByPartialName_ItThrowsAnErrorWhenMoreThanOneElementFound()
        {
            var sut = await Template.Render(
                "MultipleDuplicateIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Throws<TooManyElementsFoundException>(() => sut.GetOnly.ByPartialName("common-partial"));
        }

        [Fact]
        public async Task ByPartialName_ItReturnsNullWhenNoElementFound()
        {
            var sut = await Template.Render(
                "MultipleUniqueIds.cshtml",
                _configuration,
                new { TestText = "Hello World" });
            Assert.Null(sut.GetOnly.ByPartialName("not-real"));
        }

        [Fact]
        public async Task ItCanWithComplexClasses()
        {
            var sut = await Template.Render(
                "ComplexClasses.cshtml",
                _configuration,
                new { TestText = "Hello World" }
            );
            var expectedClasses = new[]
            {
                "cool-class-a",
                "another-class",
                "col-12"
            };
            Assert.Equal(expectedClasses, sut.GetOnly.ById("a-lot-of-classes").Classes);
        }
    }
}