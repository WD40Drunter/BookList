using BookList.Messages;
using BookList.Services;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace BookList.View
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
            
            DataContext =new AddBookWindowViewModel(Ioc.Default.GetService<IAuthorsService>()!);

            WeakReferenceMessenger.Default.Register<CloseAddingBookWindowMessage>(this, (r, m) =>
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
