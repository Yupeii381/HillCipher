using System.Threading.Tasks;

namespace Client.Services;

public interface INavigationService
{
    Task NavigateToLogin();
    Task NavigateToMain();
    Task NavigateToHistory();
}