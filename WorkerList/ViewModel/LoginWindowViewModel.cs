using BookList.Messages;
using BookList.Model;
using BookList.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookList.ViewModel
{
    internal partial class LoginWindowViewModel : ObservableObject
    {
        public LoginWindowViewModel(IUsersService usersService)
        {
            _usersService = usersService;

            LoginCommand = new RelayCommand(Login);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }
        private readonly IUsersService _usersService;

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand CloseWindowCommand { get; }

        [ObservableProperty]
        private string? _usernameValue = string.Empty;

        [ObservableProperty]
        private string? _passwordValue = string.Empty;

        public void Login()
        {
            if (string.IsNullOrWhiteSpace(UsernameValue) || string.IsNullOrWhiteSpace(PasswordValue))
            {
                MessageBox.Show("Wpisz hasło i nazwe użytkownika");
                return;
            }
            if (!_usersService.Exists(UsernameValue))
            {
                MessageBox.Show("Błędna nazwa użytkownika");
                return;
            }
            User user = _usersService.GetUser(UsernameValue)!;
            if (PasswordValue != user.Password)
            {
                MessageBox.Show("Błędne hasło");
                return;
            }
            WeakReferenceMessenger.Default.Send(new SendLoggedUserMessage(user));
            CloseWindow();
        }

        public void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new CloseLoginWindowMessage());
        }
    }
}
