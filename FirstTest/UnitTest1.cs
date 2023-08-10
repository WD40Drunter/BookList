using BookList.Model;
using BookList.Services;

namespace FirstTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        { 

            Author author = new ("Marek", "Antoniusz");


            author.FullName.Should().Be("Marek Antoniusz");
        }

        [Theory]
        [InlineData("Marek", "Antoniusz", "Marek Antoniusz")]
        [InlineData("123", "Ant2iusz", "123 Ant2iusz")]
        [InlineData("Mare", "Atoniusz", "Mare Atoniusz")]
        [InlineData("Jan", "Antoniusz", "Jan Antoniusz")]
        [InlineData("Jan", "Kowalski", "Jan Kowalski")]
        public void Test2(string firstName, string lastName, string fullName)
        {
            Author author = new (firstName, lastName);

            author.FullName.Should().Be(fullName);
        }

        [Fact]
        public void Test3()
        {
            var bookServiceStub = new Mock<IBooksService>();
            bookServiceStub.Setup(x => x.AddBook(new Book("a", 1, "1", 1, 1))).Returns(new Book("b", 1, "2", 1, 1));

            var bookService = new BooksService(null);
            var book = bookService.AddBook(new Book("a", 1, "1", 1, 1));

            book.Title.Should().Be("a");
        }
    }
}