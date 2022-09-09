using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionApi.Domain.Models;

namespace AuctionApi.Domain.Data
{
    public class AuctionDbContext : DbContext {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
        public DbSet<Admin> admins { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Item> items { get; set; }
    }
}
