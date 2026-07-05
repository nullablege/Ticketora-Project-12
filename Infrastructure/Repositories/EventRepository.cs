using Application.Abstract.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository:Repository<Event>, IEventRepository
{
    private readonly TicketoraContext _context;

    public EventRepository(TicketoraContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetUpcomingEventsWithCategoryAsync()
    {
        return await _context.Events
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted && x.Date >= DateTime.UtcNow.Date)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<List<Event>> GetEventsByCategoryIdAsync(int categoryId)
    {
        return await _context.Events
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted && x.CategoryId == categoryId)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<List<Event>> SearchEventsAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return await GetUpcomingEventsWithCategoryAsync();
        }

        searchText = searchText.Trim();

        return await _context.Events
            .Include(x => x.Category)
            .Where(x =>
                !x.IsDeleted &&
                (x.Title.Contains(searchText) ||
                 x.Description.Contains(searchText) ||
                 x.Location.Contains(searchText) ||
                 x.Organizer.Contains(searchText) ||
                 x.Category.CategoryName.Contains(searchText)))
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<Event?> GetEventDetailAsync(int id)
    {
        return await _context.Events
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Event?> GetFeaturedEventAsync()
    {
        return await _context.Events
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted && x.IsFeatured)
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync();
    }
}
