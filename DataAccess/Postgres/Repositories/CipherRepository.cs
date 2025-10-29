
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Models;

namespace HillCipher.DataAccess.Postgres.Repositories;

public class CipherRepositories
{
    private readonly CipherDbContext _context;

    public CipherRepositories(CipherDbContext context) => _context = context;

    public async Task<List<TextEntity>> GetAllTextsAsync()
    {
        return await _context.Texts.ToListAsync();
    }
}