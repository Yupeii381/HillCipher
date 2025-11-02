using HillCipher.Client.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HillCipher.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly Action _onLoginSuccess;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isBusy;

        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(IApiService apiService, Action onLoginSuccess)
        {
            _apiService = apiService;
            _onLoginSuccess = onLoginSuccess;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => !IsBusy);
            RegisterCommand = new RelayCommand(async () => await RegisterAsync(), () => !IsBusy);
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.LoginAsync(Username, Password);
                if (response?.Success == true)
                {
                    var token = response.Data?.ToString();
                    if (!string.IsNullOrEmpty(token))
                    {
                        _apiService.SetAuthToken(token);
                        _onLoginSuccess();
                    }
                }
                else
                {
                    ErrorMessage = response?.Message ?? "Login failed";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.RegisterAsync(Username, Password);
                if (response?.Success == true)
                {
                    ErrorMessage = "Registration successful! Please login.";
                }
                else
                {
                    ErrorMessage = response?.Message ?? "Registration failed";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
