using Avalonia.Controls;
using HillCipher.Client.Services;
using HillCipher.Client.ViewModels;
using HillCipher.Client.Views;

namespace HillCipher.Client
{
    public partial class MainWindow : Window
    {
        private readonly IApiService _apiService;
        private LoginViewModel _loginViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _loginViewModel = new LoginViewModel(_apiService, OnLoginSuccess);
            Content = new LoginView { DataContext = _loginViewModel };
        }

        private void OnLoginSuccess()
        {
            var mainViewModel = new MainViewModel(_apiService, OnLogout);
            Content = new MainView { DataContext = mainViewModel };
        }

        private void OnLogout()
        {
            _apiService.SetAuthToken(string.Empty);
            _loginViewModel = new LoginViewModel(_apiService, OnLoginSuccess);
            Content = new LoginView { DataContext = _loginViewModel };
        }
    }
}
