using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionApi.Domain.Models;

namespace AuctionApi.Services

{
    public interface IAuctionRepo
    {
        public IEnumerable<User> GetAllUsers();
        public IEnumerable<Item> GetAllItems();
        public IEnumerable<Admin> GetAllAdmins();
        public bool ValidLoginUser(string userName, string password);
        public bool ValidLoginAdmin(string userName, string password);
        public User AddUser(User user);
        public Item AddItem(Item item);
        public void DeleteItem(Item item);

        public void SaveChanges();
    }
}
