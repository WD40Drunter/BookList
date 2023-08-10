using BookList.Model;
using BookList.Services;
using BookList.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace BookList.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var viewModel = new MainWindowViewModel();

            DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>();
            //DataContext = new MainWindowViewModel(Ioc.Default.GetService<IBooksService>()!, Ioc.Default.GetService<IAuthorsService>()!, Ioc.Default.GetService<IRentsService>()!, Ioc.Default.GetService<IUsersService>()!);
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            //System.Net.ICredentials credentials = System.Net.CredentialCache.DefaultCredentials;
            //ReportServerCredentials reportServerCredentials = reportViewer.ServerReport.ReportServerCredentials;
            //reportServerCredentials.NetworkCredentials = credentials;

            reportViewer.ServerReport.ReportServerUrl = new Uri("http://localhost:1430/ReportServer");
            reportViewer.ServerReport.ReportPath = "/Biblioteka/Books";


            reportViewer.RefreshReport();
            //this.reportViewer.ReportPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"/Resources/Books.rdl");
            //this.reportViewer.RefreshReport();
            //this.ReportViewer.ReportPath = "\\Resources/Books.rdl";
            //this.ReportViewer.RefreshReport();

        }

        //private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    MyTextBoxWithPassword.Text = MyPasswordBox.Password;
        //}

        //private async void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    await Task.Delay(500);
        //    MyPasswordBox.Password = string.Empty;
        //}

        //public ReportParameter? SearchPhrase;
        //public ReportParameter? AuthorsPhrase;
        //private void SetReportParameters()
        //{
        //    ReportParameter[] parameters = new ReportParameter[2];
        //    SearchPhrase = new ReportParameter("SearchPhrase");
        //    AuthorsPhrase = new ReportParameter("AuthorsPhrase", "1");

        //    parameters[0] = SearchPhrase;
        //    parameters[1] = AuthorsPhrase;
        //    reportViewer.LocalReport.SetParameters(parameters);
        //}

    }
}
