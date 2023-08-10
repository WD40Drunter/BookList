using BookList.Messages;
using BookList.Model;
using BookList.Services;
using BookList.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace BookList.ViewModel
{
    public partial class MainWindowViewModel : ObservableRecipient
    {
        public MainWindowViewModel(IBooksService booksService, IAuthorsService authorsService, IRentsService rentsService, IUsersService usersService)
        {
            _booksService = booksService;
            _authorsService = authorsService;
            _rentsService = rentsService;
            _usersService = usersService;

            AddBookCommand = new RelayCommand(AddBook);
            RemoveBookCommand = new RelayCommand(RemoveBook);
            EditBookCommand = new RelayCommand(EditBook);

            AddAuthorCommand = new RelayCommand(AddAuthor);
            RemoveAuthorCommand = new RelayCommand(RemoveAuthor);
            EditAuthorCommand = new RelayCommand(EditAuthor);

            RentBookCommand = new RelayCommand(RentBook);
            RemoveRentCommand = new RelayCommand(RemoveRent);

            LoginCommand = new RelayCommand(Login);
            LogOutCommand = new RelayCommand(LogOut);
            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);

            WeakReferenceMessenger.Default.Register<FinishAddingBookMessage>(this, (r, m) =>
            {
                FinishAddingBook(m.Value);
            });

            WeakReferenceMessenger.Default.Register<EditBookMessage>(this, (r, m) =>
            {
                EditBook(m.Value);
            });

            WeakReferenceMessenger.Default.Register<FinishAddingAuthorMessage>(this, (r, m) =>
            {
                FinishAddingAuthor(m.Value);
            });

            WeakReferenceMessenger.Default.Register<EditAuthorMessage>(this, (r, m) =>
            {
                EditAuthor(m.Value);
            });

            WeakReferenceMessenger.Default.Register<SendLoggedUserMessage>(this, (r, m) =>
            {
                LoggedInUser = m.Value;
                GetUserRents();
            });

            LoadBooks();
            LoadAuthors();
            CreateBooksCollection();
            CreateAuthorsCollection();
        }

        private readonly IBooksService _booksService;

        private readonly IAuthorsService _authorsService;

        private readonly IRentsService _rentsService;

        private readonly IUsersService _usersService;

        private string? _decryptedUserPassword = string.Empty;

        public IRelayCommand AddBookCommand { get; }
        public IRelayCommand RemoveBookCommand { get; }
        public IRelayCommand EditBookCommand { get; }

        public IRelayCommand AddAuthorCommand { get; }
        public IRelayCommand RemoveAuthorCommand { get; }
        public IRelayCommand EditAuthorCommand { get; }

        public IRelayCommand RentBookCommand { get; }
        public IRelayCommand RemoveRentCommand { get; }

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand LogOutCommand { get; }
        public IRelayCommand AddUserCommand { get; }
        public IRelayCommand DeleteUserCommand { get; }

        [ObservableProperty]
        private string? _searchBookValue = string.Empty;

        [ObservableProperty]
        private Book? _selectedBook;

        [ObservableProperty]
        ICollectionView? _booksCollection;

        [ObservableProperty]
        private ObservableCollection<Book> _bookList = new();

        [ObservableProperty]
        private Author? _selectedAuthor;

        [ObservableProperty]
        private ObservableCollection<Author> _authorList = new();

        [ObservableProperty]
        ICollectionView? _authorsCollection;

        [ObservableProperty]
        private string? _searchAuthorValue = string.Empty;

        [ObservableProperty]
        private User? _loggedInUser;

        [ObservableProperty]
        private ObservableCollection<Rent> _userRentList = new();

        [ObservableProperty]
        ICollectionView? _usersRentsCollection;

        [ObservableProperty]
        private string? _searchRentValue;

        [ObservableProperty]
        private Rent? _selectedRent;

        private void LoadBooks()
        {
            BookList = new ObservableCollection<Book>(_booksService.GetAll());
        }

        private void LoadAuthors()
        {
            AuthorList = new ObservableCollection<Author>(_authorsService.GetAll());
        }

        public void CreateBooksCollection()
        {
            BooksCollection = CollectionViewSource.GetDefaultView(BookList);
            BooksCollection.SortDescriptions.Add(new SortDescription(nameof(Book.Title), ListSortDirection.Ascending));
            BooksCollection.Filter = FilterBooks;
        }

        public void CreateAuthorsCollection()
        {
            AuthorsCollection = CollectionViewSource.GetDefaultView(AuthorList);
            AuthorsCollection.SortDescriptions.Add(new SortDescription(nameof(Author.LastName), ListSortDirection.Ascending));
            AuthorsCollection.Filter = FilterAuthors;
        }

        public void CreateUserRentsCollection()
        {
            UsersRentsCollection = CollectionViewSource.GetDefaultView(UserRentList);
            //UsersRentsCollection.SortDescriptions.Add(new SortDescription(nameof(Rent.Book.Title), ListSortDirection.Ascending));
            UsersRentsCollection.Filter = FilterUserRents;
        }

        private void RefreshBooksCollection()
        {
            BooksCollection?.Refresh();
        }

        private void RefreshAuthorsCollection()
        {
            AuthorsCollection?.Refresh();
        }

        private void RefreshUserRentsCollection()
        {
            UsersRentsCollection?.Refresh();
        }

        private bool FilterBooks(object obj)
        {
            if (obj is not Book book)
            {
                return false;
            }

            return book.Title.Contains(SearchBookValue ?? string.Empty, StringComparison.InvariantCultureIgnoreCase)
                || (book.Author?.FullName ?? string.Empty).Contains(SearchBookValue ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool FilterAuthors(object obj)
        {
            if (obj is not Author author)
            {
                return false;
            }

            return author.FullName.Contains(SearchAuthorValue ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool FilterUserRents(object obj)
        {
            if (obj is not Rent rent)
            {
                return false;
            }

            return rent.Book!.Title.Contains(SearchRentValue ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        // Operacje na użytkowniku
        public bool IsLoggedIn()
        {
            if (LoggedInUser is null)
            {
                MessageBox.Show("Najpierw się zaloguj");
                return false;
            }
            return true;
        }

        public bool IsAdmin()
        {
            if (!IsLoggedIn())
            {
                return false;
            }
            if (LoggedInUser!.Privilage == "Admin")
            {
                return true;
            }
            MessageBox.Show("Brak uprawnień");
            return false;
        }

        public static void Login()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
        }

        public void LogOut()
        {
            LoggedInUser = null;
        }

        public static void AddUser()
        {
            AddUserWindow addUserWindow = new();
            addUserWindow.Show();
        }

        public void DeleteUser()
        {
            if (_rentsService.Exists(LoggedInUser!.UserId))
            {
                MessageBox.Show("Zwróć najpierw książki");
                return;
            }
            _usersService.DeleteUser(LoggedInUser!);
            LogOut();
        }

        public void GetUserRents()
        {
            if (LoggedInUser!.Privilage == "Admin")
            {
                UserRentList = new ObservableCollection<Rent>(_rentsService.GetAll());
            }
            else
            {
                UserRentList = new ObservableCollection<Rent>(_rentsService.GetUserRents(LoggedInUser!));
            }
            CreateUserRentsCollection();
        }

        // BookList modyfikowanie
        public void RemoveBook()
        {
            if (SelectedBook is null)
            {
                return;
            }
            if (!IsAdmin())
            {
                return;
            }
            if (_rentsService.Exists(SelectedBook))
            {
                MessageBox.Show("Nie można usunąć książka jest wypożyczona");
                return;
            }
            _booksService.DeleteBook(SelectedBook);
            BookList.Remove(SelectedBook);
            RefreshBooksCollection();
        }

        public void AddBook()
        {
            if (!IsLoggedIn())
            {
                return;
            }
            AddBookWindow bookWindow = new();
            bookWindow.Show();
            WeakReferenceMessenger.Default.Send(new StartAddingBookWindowMessage(LoggedInUser!.Privilage));
        }

        private void FinishAddingBook(Book book)
        {
            if (BookList.Any(x => x.Title == book.Title))
            {
                MessageBox.Show("Mamy już taką książke na liście");
                return;
            }
            Book bookWithAuthor = _booksService.AddBook(book);
            BookList.Add(bookWithAuthor);
            RefreshBooksCollection();
        }

        public void EditBookSpecial()
        {
            EditBook();
        }

        public void EditBook()
        {
            if (SelectedBook is null)
            {
                return;
            }
            if (!IsAdmin())
            {
                return;
            }
            AddBookWindow bookWindow = new();
            bookWindow.Show();

            WeakReferenceMessenger.Default.Send(new StartAddingBookWindowMessage(LoggedInUser!.Privilage));
            WeakReferenceMessenger.Default.Send(new EditBookMessage(SelectedBook));
        }

        public void EditBook(Book book)
        {
            Book bookWithAuthor = _booksService.EditBook(SelectedBook!, book);
            SelectedBook = bookWithAuthor;

            RefreshBooksCollection();
        }

        public void RentBook()
        {
            if (SelectedBook is null)
            {
                return;
            }
            if (!IsLoggedIn())
            {
                return;
            }
            if (SelectedBook.Quantity == 0)
            {
                MessageBox.Show("Brak na stanie");
                return;
            }
            
            _booksService.RentBook(SelectedBook);
            Rent rent = _rentsService.AddRent(LoggedInUser!, SelectedBook);
            UserRentList.Add(rent);

            RefreshBooksCollection();
            RefreshUserRentsCollection();
        }

        public void RemoveRent()
        {
            if(SelectedRent is null)
            {
                return;
            }
            _booksService.RentReturned(SelectedRent.Book!);
            _rentsService.DeleteRent(SelectedRent);
            UserRentList.Remove(SelectedRent);

            RefreshBooksCollection();
            RefreshUserRentsCollection();
        }

        // AuthorList modyfikowanie 
        public void RemoveAuthor()
        {
            if (SelectedAuthor is null)
            {
                return;
            }
            if (!IsAdmin())
            {
                return;
            }
            if (BookList.Any(x => x.Author == SelectedAuthor))
            {
                MessageBox.Show("Autor ma przypisane ksiązki usunięcie niemożliwe");
                return;
            }
            _authorsService.DeleteAuthor(SelectedAuthor);
            AuthorList.Remove(SelectedAuthor);
            RefreshAuthorsCollection();
        }

        public void AddAuthor()
        {
            if (!IsLoggedIn())
            {
                return;
            }
            AddAuthorWindow authorWindow = new();
            authorWindow.Show();
        }

        public void FinishAddingAuthor(Author author)
        {
            Author authorWithFullName = _authorsService.AddAuthor(author.FirstName, author.LastName);
            AuthorList.Add(authorWithFullName);
        }

        public void EditAuthor()
        {
            if (SelectedAuthor is null)
            {
                return;
            }
            if (!IsAdmin())
            {
                return;
            }
            AddAuthorWindow authorWindow = new();
            authorWindow.Show();

            WeakReferenceMessenger.Default.Send(new EditAuthorMessage(SelectedAuthor));
        }

        public void EditAuthor(Author author)
        {
            Author authorWithFullName = _authorsService.EditAuthor(SelectedAuthor!, author.FirstName, author.LastName);
            SelectedAuthor = authorWithFullName;

            RefreshAuthorsCollection();
        }

        partial void OnBookListChanged(ObservableCollection<Book> value)
        {
            RefreshBooksCollection();
        }

        partial void OnAuthorListChanged(ObservableCollection<Author> value)
        {
            RefreshAuthorsCollection();
        }

        partial void OnUserRentListChanged(ObservableCollection<Rent> value)
        {
            RefreshUserRentsCollection();
        }

        partial void OnSearchBookValueChanged(string? value)
        {
            RefreshBooksCollection();
        }

        partial void OnSearchAuthorValueChanged(string? value)
        {
            RefreshAuthorsCollection();
        }

        partial void OnSearchRentValueChanged(string? value)
        {
            RefreshUserRentsCollection();
        }
    }
}


