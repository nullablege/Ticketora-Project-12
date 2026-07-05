using Application.Abstract.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository:Repository<Category>, ICategoryRepository
{
    private readonly TicketoraContext _context;

    public CategoryRepository(TicketoraContext context) : base(context)
    {
        _context = context;
    }


    public async Task<List<Category>> GetActiveCategoriesAsync()
    {
        return await _context.Categories
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CategoryName)
            .ToListAsync();
    }
}
