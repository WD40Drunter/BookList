using BookList.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookList.Services
{
    public interface IUsersService
    {
        bool Exists(string username);
        IEnumerable<User> GetAll();
        User AddUser(User user);
        void DeleteUser(User user);
        User? GetUser(string username);
    }

    public class UsersService : IUsersService
    {
        private readonly Context _context;

        public UsersService(Context context)
        {
            _context = context;
        }

        public bool Exists(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User? GetUser(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
