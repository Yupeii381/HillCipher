namespace HillCipher.DataAccess.Postgres.Repositories;

using HillCipher.DataAccess.Postgres.Models;

public interface ITextRepository
{
    // GET
    Task<List<TextEntity>> GetAllAsync();
    Task<TextEntity?> GetByIdAsync(Guid id);
    Task<List<TextEntity>> GetByUserIdAsync(Guid id);

    // POST
    Task<TextEntity> CreateAsync(TextEntity text);

    // DELETE
    Task<bool> DeleteAsync(Guid id);
}