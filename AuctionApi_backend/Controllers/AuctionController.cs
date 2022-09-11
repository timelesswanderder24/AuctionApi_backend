﻿using Microsoft.AspNetCore.Mvc;
using AuctionApi.Domain.Models;
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
        private readonly IDistributedCache _distributedCache;

        public AuctionController(IAuctionRepo repository, IDistributedCache distributedCache)
        {
            _repository = repository;
            //.NET core has inbuilt dependency injection. So in run time it will provide the distributedCache
            _distributedCache = distributedCache;
        }

        [HttpGet("ListItems")]

        public ActionResult<IEnumerable<Item>> auctionItems()
        {

            IEnumerable<Item> activeItems = _repository.GetAllItems().Where(e => e.State == "active");
            IEnumerable<Item> sortedItems = activeItems.OrderBy(e => e.StartBid).ThenBy(e => e.Id);
            return Ok(sortedItems);
        }

        [HttpGet("GetUser")]

        public ActionResult<User> getUser(string userName)
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
    }
}
