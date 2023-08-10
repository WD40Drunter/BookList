using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookList.Model
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        public Author(string? firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string? FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public virtual List<Book>? Books { get; set; }
    }
}
