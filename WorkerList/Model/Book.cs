using System.ComponentModel.DataAnnotations;

namespace BookList.Model
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public Book(string title, decimal? rating, string? publishedYear, int authorId, int quantity)
        {
            Title = title;
            Rating = rating;
            PublishedYear = publishedYear;
            AuthorId = authorId;
            Quantity = quantity;
        }
        public string Title { get; set; }
        public decimal? Rating { get; set; }
        public string? PublishedYear { get; set; }
        public int Quantity { get; set; }

        // [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; }
    }
}
