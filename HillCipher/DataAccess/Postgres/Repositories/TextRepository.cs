using HillCipher.DataAccess.Postgres.Dtos;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HillCipher.DataAccess.Postgres.Repositories;

public interface ITextRepository
{
    Task<TextEntity?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<TextEntity>> GetAllAsync(int userId, int limit = 1000);
    Task<TextEntity> AddOrGetAsync(string content, int userId);
    Task UpdateAsync(TextEntity entity);
    Task DeleteAsync(int id, int userId);
}

public class TextRepository : ITextRepository
{
    private readonly CipherDbContext _context;

    public TextRepository(CipherDbContext context) => _context = context;

    public async Task<TextEntity?> GetByIdAsync(int id, int userId)
    {
        return await _context.Texts.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<IEnumerable<TextEntity>> GetAllAsync(int userId, int limit = 1000)
    {
        return await _context.Texts
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.UpdatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<TextEntity> AddOrGetAsync(string content, int userId)
    {
        var existing = await _context.Texts.FirstOrDefaultAsync(t => t.UserId == userId && t.Content == content);

        if (existing != null)
        {
            return existing;
        }

        var newText = new TextEntity
        {
            UserId = userId,
            Content = content
        };

        _context.Texts.Add(newText);
        await _context.SaveChangesAsync();
        return newText;
    }

    public async Task UpdateAsync(TextEntity text)
    {
        text.UpdatedAt = DateTime.UtcNow;
        _context.Texts.Update(text);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var text = await _context.Texts.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (text != null)
        {
            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();
        }
    }
}