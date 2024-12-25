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
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using VCS.FORM.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace VCS.FORM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Views.Pages.HomePages homePages;

        public MainWindow()
        {
            InitializeComponent();
            LoadUserInfo();
            
            if (homePages == null)
            {
                homePages = new Views.Pages.HomePages();
            }
            MainContent.Navigate(homePages);
        }

        private void LoadUserInfo()
        {
            if (LoginResponse.AccountInfo != null)
            {
                UserNameText.Text = LoginResponse.AccountInfo.UserName;
                UserRoleText.Text = LoginResponse.AccountInfo.AccountGroups?.FirstOrDefault()?.Name ?? "N/A";
            
             
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (homePages == null)
                homePages = new Views.Pages.HomePages();
            
            MainContent.Navigate(homePages);
        }

        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Views.Pages.CheckInPages());
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
                // Xóa thông tin đăng nhập đã lưu
                CredentialManager.DeleteCredentials();
                
                // Tạo cửa sổ đăng nhập mới
                var app = (App)Application.Current;
                var loginWindow = app.ServiceProvider.GetRequiredService<Login>();
                
                // Animation fade out cho cửa sổ hiện tại
                this.Opacity = 1;
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };
                animation.Completed += (s, _) =>
                {
                    loginWindow.Show();
                    this.Close();
                };
                this.BeginAnimation(OpacityProperty, animation);
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