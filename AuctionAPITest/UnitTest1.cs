using AuctionApi_backend.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AuctionApi.Services;
using NSubstitute;
using AuctionApi.Domain.Models;
using NUnit;
using Microsoft.Extensions.Caching.Distributed;
using AuctionApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using AuctionApi.Domain.Models.DTO;

namespace AuctionAPITest
{
    [TestFixture]
    public class UnitTest1
    {
        private IAuctionRepo _auctionRepo;
        private ILogger _logger;
        private IDistributedCache _distributedCache;

        private AuctionController _controller;

        [SetUp]
        public void SetUp()
        {
            _auctionRepo = Substitute.For<IAuctionRepo>();
            _logger = Substitute.For<ILogger>();
            var loggerFactory = Substitute.For<ILoggerFactory>();
            //Logger creation = LoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_logger);

            _controller = new AuctionController(_auctionRepo,_distributedCache, loggerFactory);
        }

        [Test]
        public void registerUserTest()
        {
            //Arrange
            User user = new User { UserName = "Anna", Password = "Key123" };
            _auctionRepo.AddUser(user);
            string expected = "User successfully registered.";

            //Act
            string result = _controller.registerUser(user);

            //Assert
            Assert.AreEqual(result, expected);
        }

        /*
        
        [Test]
        public void getUserTest()
        {
            //Arrange 
            var userName = "Anna";
            User  user = _auctionRepo.GetAllUsers().FirstOrDefault(e => e.UserName == userName);
            // ActionResult<User> expected = user;

            //Act
            User result = _controller.getUser(userName);

            //Assert
            Assert.AreEqual(result, null);

        }*/

        [Test]
        public void getAuctionItems()
        {
            //Arrange 
            IEnumerable<Item> activeItems = _auctionRepo.GetAllItems().Where(e => e.State == "active");
            IEnumerable<Item> sortedItems = activeItems.OrderBy(e => e.StartBid).ThenBy(e => e.Id);

            //Act
            IEnumerable<Item> Result = _controller.auctionItems();

            //Assert
            Assert.AreEqual(Result, sortedItems);

        }

        [Test]
        public void getAuctionItem()
        {
            //Arrange 
            int id = 24;
            IEnumerable<Item> items = _auctionRepo.GetAllItems();
            Item item = items.FirstOrDefault(x => x.Id == id);

            //Act
            Item Result = _controller.getItem(id);

            //Assert
            Assert.AreEqual(Result, item);

        }

        [Test]
        public void addItemTest()
        {
            //Arrange 
            InputItem item = new InputItem { Owner = "Katy", Description="large", StartBid=0, Title="Vase"};
            Item newItem = new Item { };
            if (item.StartBid != null)
            {
                newItem = new Item
                {
                    Owner = item.Owner,
                    Title = item.Title,
                    Description = item.Description,
                    StartBid = (float)item.StartBid,
                    State = "active"
                };
            }
            _auctionRepo.AddItem(newItem);

            //Act
            Item Result = _controller.addItem(item);

            //Assert
            Assert.AreEqual(Result, newItem);

        }

        [Test]
        public void closeAuctionTest()
        {
            //Arrange 
            int id = 24;
            IEnumerable<Item> items = _auctionRepo.GetAllItems();
            Item item = items.FirstOrDefault(x => x.Id == id);
            string expected = "";
            if (item != null)
            {
                item.State = "closed";
                _auctionRepo.SaveChanges();
                expected = "Auction closed.";
            }
            else
            {
                expected = "Auction does not exist.";
            }

            //Act
            String Result = _controller.closeAuction(id);

            //Assert
            Assert.AreEqual(Result, expected);

        }


    }
}


