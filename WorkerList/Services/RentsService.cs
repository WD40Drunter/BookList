using BookList.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookList.Services
{
    public interface IRentsService
    {
        bool Exists(int userId);
        bool Exists(Book book);
        IEnumerable<Rent> GetAll();
        IEnumerable<Rent> GetUserRents(User user);
        Rent AddRent(User user, Book book);
        void DeleteRent(Rent rent);
    }
    public class RentsService : IRentsService
    {
        private readonly Context _context;

        public RentsService(Context context)
        {
            _context = context;
        }

        public bool Exists(int userId)
        {
            return _context.Rents.Any(x => x.UserId == userId);
        }

        public bool Exists(Book book)
        {
            return _context.Rents.Any(x => x.Book == book);
        }

        public IEnumerable<Rent> GetAll()
        {
            return _context.Rents;
        }

        public IEnumerable<Rent> GetUserRents(User user)
        {
            return _context.Rents.Where(x => x.UserId == user.UserId);
        }

        public Rent AddRent(User user, Book book)
        {
            Rent rent = new (user.UserId, book.BookId);
            _context.Rents.Add(rent);
            _context.SaveChanges();
            return rent;
        }

        public void DeleteRent(Rent rent)
        {
            _context.Rents.Remove(rent);
            _context.SaveChanges();
        }

    }
}
