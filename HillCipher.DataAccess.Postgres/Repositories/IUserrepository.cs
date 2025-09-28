namespace HillCipher.DataAccess.Postgres.Repositories;

using HillCipher.DataAccess.Postgres.Models;

public interface IUserRepository
{
    // GET
    Task<List<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByEmailAsync(string email);

    // POST
    Task<UserEntity> CreateAsync(UserEntity user);

    // PATCH
    Task<UserEntity> UpdateAsync(UserEntity user);

    // DELETE
    Task<bool> DeleteAsync(Guid id);
}