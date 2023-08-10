using BookList.Messages;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace BookList.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<LoginWindowViewModel>();

            WeakReferenceMessenger.Default.Register<CloseLoginWindowMessage>(this, (r, m) =>
            {
                CloseWindow();
            });

        }

        public void CloseWindow()
        {
            Close();
        }

        private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            MyTextBoxWithPassword.Text = MyPasswordBox.Password;
        }

    }
}
