using Application.Abstract.Repository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRegistrationRepository:Repository<EventRegistration>, IEventRegistrationRepository
{
    private readonly TicketoraContext _context;

    public EventRegistrationRepository(TicketoraContext context) : base(context)
    {
        _context = context;
    }

    public async Task<EventRegistration> CreateRegistrationWithTicketAsync(EventRegistration registration, Ticket ticket)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.EventRegistrations.AddAsync(registration);
        await _context.SaveChangesAsync();

        ticket.EventRegistrationId = registration.Id;
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        registration.Tickets.Add(ticket);
        return registration;
    }

    public async Task<List<EventRegistration>> GetPastRegistrationsByUserIdAsync(int userId)
    {
        return await _context.EventRegistrations
            .Include(x => x.Event)
            .ThenInclude(x => x.Category)
            .Include(x => x.Tickets.Where(t => !t.IsDeleted))
            .Where(x =>
                !x.IsDeleted &&
                x.UserId == userId &&
                x.Event.Date < DateTime.UtcNow.Date)
            .OrderByDescending(x => x.Event.Date)
            .ToListAsync();
    }
}
