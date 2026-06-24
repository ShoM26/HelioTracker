using Heliotracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heliotracker.Services;

public class BaseService<T> : IBaseService<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}