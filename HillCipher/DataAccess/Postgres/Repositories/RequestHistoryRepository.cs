using HillCipher.DataAccess.Postgres.Dtos;
using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HillCipher.DataAccess.Postgres.Repositories;

public interface IRequestHistoryRepository
{
    Task<IEnumerable<RequestHistoryDto>> GetByUserIdAsync(int userId);
    Task AddAsync(RequestHistory history);
    Task DeleteAllByUserIdAsync(int userId);
}

public class RequestHistoryRepository : IRequestHistoryRepository
{
    private readonly CipherDbContext _context;

    public RequestHistoryRepository(CipherDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RequestHistoryDto>> GetByUserIdAsync(int userId)
    {
        var histories = await _context.RequestHistories
            .Where(h => h.UserId == userId)
            .Include(h => h.User)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();

        return histories.Select(RequestHistoryDto.FromEntity);
    }

    public async Task AddAsync(RequestHistory history)
    {
        history.CreatedAt = DateTime.UtcNow;
        _context.RequestHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllByUserIdAsync(int userId)
    {
        var histories = _context.RequestHistories.Where(h => h.UserId == userId);
        _context.RequestHistories.RemoveRange(histories);
        await _context.SaveChangesAsync();
    }
}
