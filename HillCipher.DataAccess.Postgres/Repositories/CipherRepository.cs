namespace HillCipher.DataAccess.Postgres.Repositories;

using HillCipher.DataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class CipherRepositories
{
    private readonly CipherDbContext _context;

    public CipherRepositories(CipherDbContext context) => _context = context;

    public async Task<List<TextEntity>> Get()
    {
        return await _context.Texts
            .AsNoTracking()
            .OrderBy(s => s.UserId)
            .ToListAsync();
            
    }
    public async Task<TextEntity?> GetById(Guid Id)
    {
        return await _context.Texts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == Id);

    }

    //public async Task Create(TextEntity )

}