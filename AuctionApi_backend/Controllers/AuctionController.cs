using Microsoft.AspNetCore.Mvc;
using AuctionApi.Domain.Models;
using AuctionApi.Domain.Models.DTO;
using AuctionApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AuctionApi_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuctionController:Controller
    {
        public IAuctionRepo _repository;
        private ILogger _logger;
        private readonly IDistributedCache _distributedCache;

        public AuctionController(IAuctionRepo repository, IDistributedCache distributedCache, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            //.NET core has inbuilt dependency injection. So in run time it will provide the distributedCache
            _distributedCache = distributedCache;
            _logger = loggerFactory.CreateLogger(nameof(AuctionController));
        }


        [HttpGet("ListItems")]

        public IEnumerable<Item> auctionItems()
        {

            IEnumerable<Item> activeItems = _repository.GetAllItems().Where(e => e.State == "active");
            IEnumerable<Item> sortedItems = activeItems.OrderBy(e => e.StartBid).ThenBy(e => e.Id);
            return (sortedItems);
        }

        [HttpGet("GetUser")]

        public User getUser(string userName)
        {
            User user = null;
            // get data from Redis Cache
            string userStr = _distributedCache.GetString("userObj_" + userName);

            //if the user is in the cache get it
            if (!string.IsNullOrEmpty(userStr))
            {
                user = JsonConvert.DeserializeObject<User>(userStr);
            }
            else
            {
                //Get user from the database
                user = _repository.GetAllUsers().FirstOrDefault(e => e.UserName == userName) ;
                //store the user into the cache  
                if (user != null)
                   _distributedCache.SetStringAsync("userObj_" + userName, JsonConvert.SerializeObject(user));
            }
            /*
            if (user == null)
                return NotFound;*/

            return user;
        }

        // PUT /api/Register
        [HttpPost("Register")]
        public string registerUser(User user)
        {
            String output = "";
            IEnumerable<User> users = _repository.GetAllUsers();
            if (users.FirstOrDefault(u => u.UserName == user.UserName) == null)
            {
                _repository.AddUser(user);
                output = "User successfully registered.";
            }
            else
            {
                output = "Username not available.";
            }

            return (output);

        }

        // GET /api/GetItem/{id}
        [HttpGet("GetItem/{id}")]
        public Item getItem(int id)
        {
            IEnumerable<Item> items = _repository.GetAllItems();
            Item item = items.FirstOrDefault(x => x.Id == id);
            return (item);
        }

        // PUT /api/AddItem
        [HttpPost("AddItem")]
        public Item addItem(InputItem inputItem)
        {
            Item item;

            if (inputItem.StartBid == null)
            {
                item = new Item
                {
                    Owner = inputItem.Owner,
                    Title = inputItem.Title,
                    Description = inputItem.Description,
                    StartBid = 0,
                    State = "active"
                };
            }
            else
            {
                item = new Item
                {
                    Owner = inputItem.Owner,
                    Title = inputItem.Title,
                    Description = inputItem.Description,
                    StartBid = (float)inputItem.StartBid,
                    State = "active"
                };
            }
            _repository.AddItem(item);
            Response.Headers.Add("location", "https://localhost:8080/api/GetItem/" + item.Id);
            return (item);
        }

        [HttpGet("CloseAuction/{id}")]

        public string closeAuction(int id)
        {
            string result = "";
            String userName;
            IEnumerable<Item> items = _repository.GetAllItems();
            Item item = items.FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                item.State = "closed";
                _repository.SaveChanges();
                result = "Auction closed.";
                }
            else
            {
                result = "Auction does not exist.";
            }

            return (result);
        }


    }
}
