using System.Threading.Tasks;
using Client.Models;

namespace Client.Services;

public interface IApiService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task<string> RegisterAsync(RegisterRequest registerRequest);
    Task<int> AddTextAsync(AddTextRequest addTextRequest);
    Task<bool> UpdateTextAsync(UpdateTextRequest updateTextRequest);
    Task<HistoryItem[]> GetHistoryAsync();

}