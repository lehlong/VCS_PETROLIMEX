using DMS.BUSINESS.Dtos.Auth;
using DMS.BUSINESS.Services.Auth;
using DMS.CORE;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VCS.FORM.Utilities;

namespace VCS.FORM
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly IAuthService _service;
        private readonly AppDbContext _dbContext;

        public Login(IAuthService service, AppDbContext dbContext)
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEnterKey);
            
            _service = service;
            _dbContext = dbContext;

            // Kiểm tra và tự động điền thông tin đăng nhập đã lưu
            LoadSavedCredentials();
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, new RoutedEventArgs());
            }
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        // Thêm các hàm xử lý sự kiện còn thiếu
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassWord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void LoadSavedCredentials()
        {
            var savedCredentials = CredentialManager.GetSavedCredentials();
            if (savedCredentials != null)
            {
                txtEmail.Text = savedCredentials.Username;
                txtPassword.Password = savedCredentials.Password;
                RememberMe.IsChecked = savedCredentials.RememberMe;

                // Tự động đăng nhập nếu có thông tin được lưu
                Button_Click(this, new RoutedEventArgs());
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // Kiểm tra kết nối database
                bool canConnect = await _dbContext.Database.CanConnectAsync();
                if (!canConnect)
                {
                    MessageBox.Show("Không thể kết nối đến database!", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_service == null)
                {
                    MessageBox.Show("Lỗi khởi tạo service!", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var loginDto = new LoginDto
                {
                    UserName = txtEmail.Text,
                    Password = txtPassword.Password
                };

                var result = await _service.Login(loginDto);

                if (result != null)
                {
                    // Lưu thông tin đăng nhập nếu người dùng chọn "Ghi nhớ đăng nhập"
                    if (RememberMe.IsChecked == true)
                    {
                        CredentialManager.SaveCredentials(new LoginCredentials
                        {
                            Username = txtEmail.Text,
                            Password = txtPassword.Password,
                            RememberMe = true
                        });
                    }
                    else
                    {
                        CredentialManager.DeleteCredentials();
                    }
                    LoginResponse.AccessToken = result.AccessToken;
                    LoginResponse.ExpireDate = result.ExpireDate;
                    LoginResponse.RefreshToken = result.RefreshToken; 
                    LoginResponse.ExpireDateRefreshToken = result.ExpireDateRefreshToken;
                    LoginResponse.AccountInfo = result.AccountInfo;
                    MainWindow mainWindow = new MainWindow();

                    // Animation fade out cho cửa sổ login
                    this.Opacity = 1;
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.0,
                        Duration = new Duration(TimeSpan.FromSeconds(0.3))
                    };
                    animation.Completed += (s, _) =>
                    {
                        mainWindow.Show();
                        this.Close();
                        
                    };
                    this.BeginAnimation(OpacityProperty, animation);
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
