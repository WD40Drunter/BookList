using System.ComponentModel.DataAnnotations;

namespace BookList.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public User(string? firstName, string? lastName, string username, string password, string privilage)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Privilage = privilage;  
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Privilage { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
