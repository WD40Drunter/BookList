using BookList.Messages;
using BookList.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace BookList.ViewModel
{
    internal partial class AddAuthorWindowViewModel : ObservableObject
    {
        public AddAuthorWindowViewModel()
        {
            FinishActionCommand = new RelayCommand(FinishAction);
            CloseWindowCommand = new RelayCommand(CloseWindow);

            WeakReferenceMessenger.Default.Register<EditAuthorMessage>(this, (r, m) =>
            {
                EditAuthor(m.Value);
            });
        }
        private bool _isEditCalled = false;

        public IRelayCommand FinishActionCommand { get; }
        public IRelayCommand CloseWindowCommand { get; }

        [ObservableProperty]
        private string? _authorFirstName = string.Empty;

        [ObservableProperty]
        private string? _authorLastName = string.Empty;

        public bool ValidateAuthorLastName()
        {
            if (string.IsNullOrEmpty(AuthorLastName))
            {
                MessageBox.Show("Nazwisko autora nie może być puste");
                return false;
            }
            return true;
        }

        public void FinishAction()
        {
            if (!ValidateAuthorLastName())
            {
                return;
            }

            var author = new Author(AuthorFirstName, AuthorLastName ?? string.Empty);
            if (_isEditCalled)
            {
                WeakReferenceMessenger.Default.Send(new EditAuthorMessage(author));
                _isEditCalled = false;
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new FinishAddingAuthorMessage(author));
            }
            CloseWindow();
        }

        public void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new CloseAddingAuthorWindowMessage());
        }

        public void EditAuthor(Author author)
        {
            AuthorFirstName = author.FirstName;
            AuthorLastName = author.LastName;
            _isEditCalled = true;
        }
    }
}
