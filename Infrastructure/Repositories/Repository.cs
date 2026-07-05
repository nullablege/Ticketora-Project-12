using Application.Abstract.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly TicketoraContext _context;
    
    public Repository(TicketoraContext context)
    {
        _context = context;
    }
    
    
    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    
}
