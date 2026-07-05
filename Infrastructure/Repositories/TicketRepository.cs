using Application.Abstract.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository:Repository<Ticket>, ITicketRepository
{
    private readonly TicketoraContext _context;

    public TicketRepository(TicketoraContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetActiveTicketsByUserIdAsync(int userId)
    {
        return await _context.Tickets
            .Include(x => x.EventRegistration)
            .ThenInclude(x => x.Event)
            .ThenInclude(x => x.Category)
            .Where(x =>
                !x.IsDeleted &&
                !x.EventRegistration.IsDeleted &&
                x.EventRegistration.UserId == userId &&
                x.EventRegistration.Event.Date >= DateTime.UtcNow.Date)
            .OrderBy(x => x.EventRegistration.Event.Date)
            .ToListAsync();
    }

    public async Task<List<Ticket>> GetRecentActiveTicketsByUserIdAsync(int userId, int count)
    {
        return await _context.Tickets
            .Include(x => x.EventRegistration)
            .ThenInclude(x => x.Event)
            .ThenInclude(x => x.Category)
            .Where(x =>
                !x.IsDeleted &&
                !x.EventRegistration.IsDeleted &&
                x.EventRegistration.UserId == userId &&
                x.EventRegistration.Event.Date >= DateTime.UtcNow.Date)
            .OrderBy(x => x.EventRegistration.Event.Date)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Ticket?> GetTicketDetailByIdAsync(int ticketId, int userId)
    {
        return await _context.Tickets
            .Include(x => x.EventRegistration)
            .ThenInclude(x => x.Event)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x =>
                x.Id == ticketId &&
                !x.IsDeleted &&
                !x.EventRegistration.IsDeleted &&
                x.EventRegistration.UserId == userId);
    }

    public async Task<int> GetActiveTicketCountByUserIdAsync(int userId)
    {
        return await _context.Tickets
            .CountAsync(x =>
                !x.IsDeleted &&
                !x.EventRegistration.IsDeleted &&
                x.EventRegistration.UserId == userId &&
                x.EventRegistration.Event.Date >= DateTime.UtcNow.Date);
    }
}
