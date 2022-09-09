using Microsoft.AspNetCore.Mvc;
using AuctionApi.Domain.Models;
using AuctionApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuctionApi_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuctionController:Controller
    {
        public IAuctionRepo _repository;

        public AuctionController(IAuctionRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("ListItems")]

        public ActionResult<IEnumerable<Item>> auctionItems()
        {
            IEnumerable<Item> activeItems = _repository.GetAllItems().Where(e => e.State == "active");
            IEnumerable<Item> sortedItems = activeItems.OrderBy(e => e.StartBid).ThenBy(e => e.Id);
            return Ok(sortedItems);
        }
    }
}
