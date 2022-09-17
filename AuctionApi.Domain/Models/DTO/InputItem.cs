using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AuctionApi.Domain.Models.DTO
{
    public class InputItem
    {
        [Required]
        public string Owner { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public float? StartBid { get; set; }
    }
}
