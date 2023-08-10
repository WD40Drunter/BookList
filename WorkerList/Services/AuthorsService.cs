using BookList.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookList.Services
{
    public interface IAuthorsService
    {
        bool Exists(string authorLastName);
        IEnumerable<Author> GetAll();
        void DeleteAuthor(Author author);
        Author AddAuthor(string? authorFirstName, string authorLastName);
        Author EditAuthor(Author author, string? authorFirstName, string authorLastName);
    }

    public class AuthorsService : IAuthorsService
    {
        private readonly Context _context;
        public AuthorsService(Context context)
        {
            _context = context;
        }
        public bool Exists(string authorLastName)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public IEnumerable<Author> GetAll()
        {
            return _context.Authors;
        }

        public Author AddAuthor(string? authorFirstName, string authorLastName)
        {
            Author author = new(authorFirstName, authorLastName);
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        public Author EditAuthor(Author author, string? authorFirstName, string authorLastName)
        {
            author.FirstName = authorFirstName;
            author.LastName = authorLastName;

            _context.SaveChanges();
            return author;
        }
    }
}
