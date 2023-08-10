using BookList.Messages;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace BookList.View
{
    /// <summary>
    /// Interaction logic for AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        public AddAuthorWindow()
        {
            InitializeComponent();

            DataContext = new AddAuthorWindowViewModel();//Ioc.Default.GetService<IAuthorsService>()!);

            WeakReferenceMessenger.Default.Register<CloseAddingAuthorWindowMessage>(this, (r, m) =>
            {
                Close();
            });
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
