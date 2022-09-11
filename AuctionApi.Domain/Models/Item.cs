using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AuctionApi.Domain.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        //public User Owner { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public float StartBid { get; set; }
        public float CurrentBid { get; set; }
        //public User Buyer { get; set; } = default;

        public string State { get; set; }
    }
}
