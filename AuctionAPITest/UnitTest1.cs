using AuctionApi_backend.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionAPITest
{
    public class UnitTest1
    {
        private readonly string[] args;

        [Test]
        public void putArtist()
        {
            //Arrange
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            var app = builder.Build();
            ILogger<AuctionController> logger = app.Services.GetRequiredService<ILogger<AuctionController>>();
            var test = new AuctionController(logger);

            //Act
            var count = test.Get().Count();
            var actionResult = test.Post("Jane", 13, "Rock");
            //Assert
            Assert.AreEqual(count, test.Get().Count());

        }
        [Test]
        public void GetReturnsProduct()
        {
            // Arrange
            var controller = new AuctionController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            Product product;
            Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            Assert.AreEqual(10, product.Id);
        }

        /*
        [Test]
        public void deleteArtist()
        {
            //Arrange
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            var app = builder.Build();
            ILogger<ArtistController> logger = app.Services.GetRequiredService<ILogger<ArtistController>>();
            var test = new ArtistController(logger);

            //Act
            var count = test.Get().Count();
            var actionResult = test.DemonstrateDelete(test.Get().ElementAt(0).Name);
            //Assert
            Assert.AreEqual(count, test.Get().Count());

        }*/
    }
}