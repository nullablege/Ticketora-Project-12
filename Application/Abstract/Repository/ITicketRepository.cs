using Domain.Entities;

namespace Application.Abstract.Repository;

public interface ITicketRepository:IRepository<Ticket>
{
    Task<List<Ticket>> GetActiveTicketsByUserIdAsync(int userId);
    Task<List<Ticket>> GetRecentActiveTicketsByUserIdAsync(int userId, int count);
    Task<Ticket?> GetTicketDetailByIdAsync(int ticketId, int userId);
    Task<int> GetActiveTicketCountByUserIdAsync(int userId);
}