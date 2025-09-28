namespace HillCipher.DataAccess.Postgres.Repositories;

using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class UserRepository : IUserRepository
{
    private readonly CipherDbContext _context;

    public UserRepository(CipherDbContext context) => _context = context;

    public async Task<List<UserEntity>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }
    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Users
             .AsNoTracking()
             .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _context.Users
             .AsNoTracking()
             .FirstOrDefaultAsync(t => t.Email == email);
    }

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.Now;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(t => t.Id == user.Id);
        if (existing == null)
            throw new ArgumentException($"User with id {user.Id} not found");

        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
        if (user == null) return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}