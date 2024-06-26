﻿using BookList.Messages;
using BookList.Services;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookList.View
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();

            DataContext = new AddUserWindowViewModel(Ioc.Default.GetService<IUsersService>()!);

            WeakReferenceMessenger.Default.Register<CloseAddingUserWindowMessage>(this, (r, m) =>
            {
                CloseWindow();
            });
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
