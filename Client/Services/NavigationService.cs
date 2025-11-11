using Client;
using Client.Services;
using System.Threading.Tasks;

namespace Client.Services;

public class NavigationService : INavigationService
{
    public Task NavigateToLogin() => App.ShowLoginWindowAsync();
    public Task NavigateToMain() => App.ShowMainWindowAsync();
    public Task NavigateToHistory() => App.ShowHistoryWindowAsync();
}