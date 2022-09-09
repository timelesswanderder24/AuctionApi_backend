using Microsoft.EntityFrameworkCore.ChangeTracking;
using AuctionApi.Domain.Models;
using AuctionApi.Domain.Data;


namespace AuctionApi.Services
{
    public class AuctionRepo : IAuctionRepo
    {
        private readonly AuctionDbContext _dbContext;

        public AuctionRepo(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> Users = _dbContext.users.ToList<User>();
            return Users;
        }

        public IEnumerable<Item> GetAllItems()
        {
            IEnumerable<Item> Items = _dbContext.items.ToList<Item>();
            return Items;
        }

        public IEnumerable<Admin> GetAllAdmins()
        {
            IEnumerable<Admin> Admins = _dbContext.admins.ToList<Admin>();
            return Admins;
        }

        public User AddUser(User user)
        {
            EntityEntry<User> e = _dbContext.users.Add(user);
            User c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }

        public Item AddItem(Item item)
        {
            EntityEntry<Item> e = _dbContext.items.Add(item);
            Item c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }

        public void DeleteItem(Item item)
        {
            _dbContext.items.Remove(item);
            _dbContext.SaveChanges();
        }

        public bool ValidLoginUser(string userName, string password)
        {
            User u = _dbContext.users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
            if (u == null)
                return false;
            else
                return true;
        }

        public bool ValidLoginAdmin(string userName, string password)
        {
            Admin u = _dbContext.admins.FirstOrDefault(e => e.UserName == userName && e.Password == password);
            if (u == null)
                return false;
            else
                return true;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }


    }
}