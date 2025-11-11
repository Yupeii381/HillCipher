using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Client.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HillCipher.Client.ViewModels;

[ObservableObject]
public partial class MainWindowViewModel
{
    private readonly IApiService _api;
    private readonly ITokenStorage _tokenStorage;
    private readonly INavigationService _nav;

    [ObservableProperty] private string _inputText = "никогда еще штирлиц не был так близок к провалу";
    [ObservableProperty] private string _key = "кодовое слово же";
    [ObservableProperty] private string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя., ?";
    [ObservableProperty] private string _resultText = "";
    [ObservableProperty] private bool _isEncryptMode = true;

    private int? _currentTextId;
    private bool _isTextModified = true;

    public MainWindowViewModel(IApiService api, ITokenStorage tokenStorage, INavigationService nav)
    {
        _api = api;
        _tokenStorage = tokenStorage;
        _nav = nav;

        this.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(InputText)) _isTextModified = true;
        };
    }

    [RelayCommand]
    private async Task AddText()
    {
        if (string.IsNullOrWhiteSpace(InputText))
        {
            ResultText = "⚠️ Текст не может быть пустым";
            return;
        }

        try
        {
            var id = await _api.AddTextAsync(InputText);
            _currentTextId = id;
            _isTextModified = false;
            ResultText = $"✅ Текст сохранён (ID: {id})";
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await LogoutAndGoToLogin();
        }
        catch (Exception ex)
        {
            ResultText = $"❌ {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task Calculate()
    {
        if (_currentTextId == null)
        {
            ResultText = "⚠️ Сначала добавьте текст (кнопка «Добавить»)";
            return;
        }

        if (_isTextModified)
        {
            ResultText = "⚠️ Текст изменён. Нажмите «Добавить», чтобы обновить.";
            return;
        }

        try
        {
            var result = await _api.CryptAsync(_currentTextId.Value, Key, IsEncryptMode);
            ResultText = result;
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            ResultText = $"❌ Ошибка ключа или алфавита: {SanitizeError(ex.ResponseBody)}";
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await LogoutAndGoToLogin();
        }
        catch (Exception ex)
        {
            ResultText = $"❌ {ex.Message}";
        }
    }

    private string SanitizeError(string? s) =>
        (s?.Trim('"', '{', '}', ' ') ?? "неизвестная ошибка").Substring(0, Math.Min(100, s?.Length ?? 10));

    [RelayCommand]
    private Task ShowHistory() => _nav.NavigateToHistory();

    [RelayCommand]
    private async Task Logout()
    {
        await LogoutAndGoToLogin();
    }

    private async Task LogoutAndGoToLogin()
    {
        _tokenStorage.Clear();
        _api.SetAuthToken(null);
        await _nav.NavigateToLogin();
    }
}