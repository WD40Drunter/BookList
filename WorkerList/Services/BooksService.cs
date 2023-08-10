using BookList.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookList.Services
{
    public interface IBooksService
    {
        bool Exists(string bookTitle);
        IEnumerable<Book> GetAll();
        Book AddBook(Book changeValue);
        void DeleteBook(Book book);
        Book EditBook(Book book, Book changeValue);
        void RentBook(Book selectedBook);
        void RentReturned(Book selectedBook);
        //int CheckForAuthorId(string? authorFirstName, string authorLastName);
    }

    public class BooksService : IBooksService
    {
        private readonly Context _context;

        public BooksService(Context context)
        {
            _context = context;
        }

        public bool Exists(string bookTitle)
        {
            return _context.Books.Any(x => x.Title == bookTitle);
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.Include(x => x.Author);
        }

        public Book AddBook(Book changeValue)
        {
            //int authorId = CheckForAuthorId(changeValue.AuthorFirstName, changeValue.AuthorLastName);
            Book book = new(changeValue.Title, changeValue.Rating, changeValue.PublishedYear, changeValue.AuthorId, changeValue.Quantity);
            if (_context is not null)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            return book;
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public Book EditBook(Book selectedBook, Book changeValue)
        {
            //int authorId = CheckForAuthorId(changeValue.AuthorFirstName, changeValue.AuthorLastName);
            //_context.Books.Update(book);
            selectedBook.Title = changeValue.Title;
            selectedBook.Rating = changeValue.Rating;
            selectedBook.PublishedYear = changeValue.PublishedYear;
            selectedBook.AuthorId = changeValue.AuthorId;
            selectedBook.Quantity = changeValue.Quantity;

            _context.SaveChanges();
            return selectedBook;
        }

        public void RentBook(Book selectedBook)
        {
            selectedBook.Quantity--;
            _context.SaveChanges();
        }

        public void RentReturned(Book book)
        {
            book.Quantity++;
            _context.SaveChanges();
        }

        //public int CheckForAuthorId(string? authorFirstName, string authorLastName)
        //{
        //    int authorId;
        //    if (!_context.Authors.Any(x => x.FirstName == authorFirstName && x.LastName == authorLastName))
        //    {
        //        Author author = new(authorFirstName, authorLastName);
        //        _context.Authors.Add(author);
        //        _context.SaveChanges();

        //        return author.AuthorId;
        //    }
        //    //authorId = _context.Authors.FirstOrDefault(z => $"{z.FirstName} {z.LastName}" == $"{authorFirstName} {authorLastName}")?.AuthorId ?? 0;
        //    authorId = _context.Authors.FirstOrDefault(z => z.FirstName == authorFirstName && z.LastName == authorLastName)?.AuthorId ?? 0;
        //    //authorId = oneAuthor.Select(x => x.AuthorId).FirstOrDefault();
        //    return authorId;
        //}
    }
}
