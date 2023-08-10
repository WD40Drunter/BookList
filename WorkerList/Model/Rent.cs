using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookList.Model
{
    public class Rent
    {
        [Key]
        public int RentId { get; set; }

        public Rent(int userId, int bookId)
        {
            UserId = userId;
            BookId = bookId;
        }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

    }
}
