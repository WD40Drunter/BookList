using BookList.Messages;
using BookList.Model;
using BookList.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace BookList.ViewModel
{
    internal partial class AddBookWindowViewModel : ObservableObject
    {
        public AddBookWindowViewModel(IAuthorsService authorsService)
        {
            FinishActionCommand = new RelayCommand(FinishAction);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            _authorsService = authorsService;

            WeakReferenceMessenger.Default.Register<EditBookMessage>(this, (r, m) =>
            {
                EditBook(m.Value);
            });

            WeakReferenceMessenger.Default.Register<StartAddingBookWindowMessage>(this, (r, m) =>
            {
                if (m.Value == "Admin")
                {
                    IsAdmin = true;
                }
                else
                {
                    IsAdmin = false;
                }
            });

            LoadAuthors();
            CreateAuthorsCollection();
        }
        private readonly IAuthorsService _authorsService;

        private bool _isEditCalled = false;

        public IRelayCommand FinishActionCommand { get; }
        public IRelayCommand CloseWindowCommand { get; }

        [ObservableProperty]
        private bool _isAdmin = false;

        [ObservableProperty]
        private string? _bookTitle = string.Empty;

        [ObservableProperty]
        private string? _bookRating = string.Empty;

        [ObservableProperty]
        private string? _bookPublishedYear = string.Empty;

        [ObservableProperty]
        private Author? _selectedAuthorForBook;

        [ObservableProperty]
        private string? _bookQuantity = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Author> _authorList = new();

        [ObservableProperty]
        ICollectionView? _authorsCollection;

        public decimal? ValidateRating()
        {
            if ((BookRating ?? string.Empty).Contains('.'))
            {
                BookRating = BookRating?.Replace('.', ',');
            }
            decimal? bookRating = null;
            if (decimal.TryParse(BookRating, out decimal resultOfParsing))
            {
                bookRating = resultOfParsing;
            }
            return bookRating;
        }

        public void ValidatePublishedYear()
        {
            if (BookPublishedYear?.Length > 4)
            {
                BookPublishedYear = BookPublishedYear[..4];
            }
            if (!int.TryParse(BookPublishedYear, out int resultOfParsing))
            {
                BookPublishedYear = string.Empty;
                return;
            }
            BookPublishedYear = Convert.ToString(resultOfParsing);
        }

        public bool ValidateIfBookCanBeAdded()
        {
            if (string.IsNullOrEmpty(BookTitle))
            {
                MessageBox.Show("Nazwa książki nie może być pusta");
                return false;
            }
            if (SelectedAuthorForBook is null)
            {
                MessageBox.Show("Musisz wybrać autora książki");
                return false;
            }
            return true;
        }

        public int ValidateQuantity()
        {
            if (int.TryParse(BookQuantity, out int result))
            {
                return result;
            }
            return 0;
        }

        public void FinishAction()
        {
            if (!ValidateIfBookCanBeAdded())
            {
                return;
            }
            decimal? bookRating = ValidateRating();
            ValidatePublishedYear();
            int bookQuantity = ValidateQuantity();

            var book = new Book(BookTitle ?? string.Empty, bookRating, BookPublishedYear, SelectedAuthorForBook!.AuthorId, bookQuantity);
            if (_isEditCalled)
            {
                WeakReferenceMessenger.Default.Send(new EditBookMessage(book));
                _isEditCalled = false;
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new FinishAddingBookMessage(book));
            }
            CloseWindow();
        }

        public void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new CloseAddingBookWindowMessage());
        }

        public void EditBook(Book book)
        {
            BookTitle = book.Title;
            BookRating = book.Rating.ToString();
            BookPublishedYear = book.PublishedYear;
            SelectedAuthorForBook = book.Author;
            BookQuantity = book.Quantity.ToString();
            _isEditCalled = true;
        }

        public void LoadAuthors()
        {
            AuthorList = new ObservableCollection<Author>(_authorsService.GetAll());
        }

        public void CreateAuthorsCollection()
        {
            AuthorsCollection = CollectionViewSource.GetDefaultView(AuthorList);
            AuthorsCollection.SortDescriptions.Add(new SortDescription(nameof(Author.LastName), ListSortDirection.Ascending));
        }
    }
}
