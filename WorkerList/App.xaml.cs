using BookList.Model;
using BookList.Services;
using BookList.View;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.IO;
using System.Windows;

namespace BookList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            var cs = Configuration.GetConnectionString("Library");

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddDbContext<Context>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Library"));
                })
                    .AddScoped<IBooksService, BooksService>()
                    .AddScoped<IAuthorsService, AuthorsService>()
                    .AddScoped<IRentsService, RentsService>()
                    .AddScoped<IUsersService, UsersService>()
                    .AddTransient<AddBookWindow>()
                    .AddTransient<AddAuthorWindow>()
                    .AddTransient<MainWindowViewModel>()
                    .AddTransient<AddBookWindowViewModel>()
                    .AddTransient<LoginWindowViewModel>()
                    .BuildServiceProvider()
                );
        }
    }
}
