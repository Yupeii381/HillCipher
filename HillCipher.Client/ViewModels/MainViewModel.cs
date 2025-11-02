using HillCipher.Client.Models;
using HillCipher.Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HillCipher.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private string _newText = string.Empty;
        private string _key = "GYBNQKURP";
        private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _result = string.Empty;
        private TextItem? _selectedText;
        private bool _isBusy;

        public ObservableCollection<TextItem> Texts { get; } = new();

        public string NewText
        {
            get => _newText;
            set => SetField(ref _newText, value);
        }

        public string Key
        {
            get => _key;
            set => SetField(ref _key, value);
        }

        public string Alphabet
        {
            get => _alphabet;
            set => SetField(ref _alphabet, value);
        }

        public string Result
        {
            get => _result;
            set => SetField(ref _result, value);
        }

        public TextItem? SelectedText
        {
            get => _selectedText;
            set => SetField(ref _selectedText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        public ICommand AddTextCommand { get; }
        public ICommand EncryptCommand { get; }
        public ICommand DecryptCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(IApiService apiService, Action onLogout)
        {
            _apiService = apiService;
            AddTextCommand = new RelayCommand(async () => await AddTextAsync(), () => !IsBusy && !string.IsNullOrWhiteSpace(NewText));
            EncryptCommand = new RelayCommand(async () => await EncryptAsync(), () => !IsBusy && SelectedText != null);
            DecryptCommand = new RelayCommand(async () => await DecryptAsync(), () => !IsBusy && SelectedText != null);
            RefreshCommand = new RelayCommand(async () => await LoadTextsAsync(), () => !IsBusy);
            LogoutCommand = new RelayCommand(onLogout);

            _ = LoadTextsAsync();
        }

        private async Task LoadTextsAsync()
        {
            IsBusy = true;
            try
            {
                var response = await _apiService.GetTextsAsync();
                if (response?.Success == true)
                {
                    Texts.Clear();
                    if (response.Data != null)
                    {
                        foreach (var text in response.Data)
                        {
                            Texts.Add(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texts: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddTextAsync()
        {
            IsBusy = true;
            try
            {
                var response = await _apiService.AddTextAsync(NewText);
                if (response?.Success == true && response.Data != null)
                {
                    Texts.Add(response.Data);
                    NewText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding text: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task EncryptAsync()
        {
            if (SelectedText == null) return;

            IsBusy = true;
            Result = string.Empty;
            try
            {
                var response = await _apiService.EncryptTextAsync(SelectedText.Id, Key, Alphabet);
                if (response?.Success == true)
                {
                    Result = response.Data?.Encrypted ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Result = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DecryptAsync()
        {
            if (SelectedText == null) return;

            IsBusy = true;
            Result = string.Empty;
            try
            {
                var response = await _apiService.DecryptTextAsync(SelectedText.Id, Key, Alphabet);
                if (response?.Success == true)
                {
                    Result = response.Data?.Encrypted ?? string.Empty; // Note: API returns "Encrypted" property for both
                }
            }
            catch (Exception ex)
            {
                Result = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
