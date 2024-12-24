using DMS.BUSINESS.Services.MD;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCS.FORM.Views.Pages;

namespace VCS.FORM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Views.Pages.Home home;

        public MainWindow()
        {
            InitializeComponent();
            LoadUserInfo();
            
            //home = new Views.Pages.Home();
            //MainContent.Navigate(home);
        }

        private void LoadUserInfo()
        {
            // TODO: Lấy thông tin user từ service authentication
            UserNameText.Text = "Nguyễn Văn A";
            UserRoleText.Text = "Admin";
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (home == null)
            //    home = new Views.Pages.Home();

            //MainContent.Navigate(home);
            MainContent.Navigate(new Views.Pages.Home());
        }

        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Navigate(new DocumentPage());
        }

        private void FinanceButton_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Navigate(new FinancePage());
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Navigate(new ReportPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //Login loginWindow = new Login();
                //loginWindow.Show();
                this.Close();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? 
                              WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Implement search functionality
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}