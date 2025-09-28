namespace HillCipher.DataAccess.Postgres.Repositories;

using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class TextRepository : ITextRepository
{
    private readonly CipherDbContext _context;

    public TextRepository(CipherDbContext context) => _context = context;

    public async Task<List<TextEntity>> GetAllAsync()
    {
        return await _context.Texts
            .AsNoTracking()
            .Include(t => t.UserId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }
    public async Task<TextEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Texts
            .AsNoTracking()
            .Include(t => t.UserId)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TextEntity>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Texts
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }
    public async Task<TextEntity> CreateAsync(TextEntity text)
    {
        text.Id = Guid.NewGuid();
        text.CreatedAt = DateTime.Now;

        _context.Texts.Add(text);
        await _context.SaveChangesAsync();
        return text;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var text = await _context.Texts.FirstOrDefaultAsync(t => t.Id == id);
        if (text == null) return false;
        _context.Texts.Remove(text);
        await _context.SaveChangesAsync();
        return true;
    }
}