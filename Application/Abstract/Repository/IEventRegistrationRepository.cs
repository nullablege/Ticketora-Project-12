using Domain.Entities;

namespace Application.Abstract.Repository;

public interface IEventRegistrationRepository:IRepository<EventRegistration>
{
    Task<EventRegistration> CreateRegistrationWithTicketAsync(EventRegistration registration, Ticket ticket);
    Task<List<EventRegistration>> GetPastRegistrationsByUserIdAsync(int userId);

}