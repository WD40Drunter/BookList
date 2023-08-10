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
    internal partial class AddUserWindowViewModel: ObservableObject
    {
        public AddUserWindowViewModel(IUsersService usersService)
        {
            _usersService = usersService;

            AddUserCommand = new RelayCommand(AddUser);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }
        private readonly IUsersService _usersService;

        public IRelayCommand AddUserCommand { get; }
        public IRelayCommand CloseWindowCommand { get; }

        [ObservableProperty]
        private string? _userFirstNameValue = string.Empty;

        [ObservableProperty]
        private string? _userLastNameValue = string.Empty;

        [ObservableProperty]
        private string _usernameValue = string.Empty;

        [ObservableProperty]
        private string _passwordValue = string.Empty;
        
        [ObservableProperty]
        private string _privilageValue = string.Empty;

        public void AddUser()
        {
            if(string.IsNullOrWhiteSpace(UsernameValue) || string.IsNullOrWhiteSpace(PasswordValue)) 
            {
                MessageBox.Show("Wpisz nazwę użytkownika i hasło");
            }
            if (_usersService.Exists(UsernameValue))
            {
                MessageBox.Show("Login zajęty");
                return;
            }
            if (string.IsNullOrEmpty(PrivilageValue))
            {
                PrivilageValue = "Standard";
            }
            User user = new(UserFirstNameValue, UserLastNameValue, UsernameValue, PasswordValue, PrivilageValue);
            _usersService.AddUser(user);
            CloseWindow();
        }


        public void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new CloseAddingUserWindowMessage());
        }

    }
}
