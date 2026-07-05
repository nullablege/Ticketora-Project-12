using Domain.Entities;

namespace Application.Abstract.Repository;

public interface IEventRepository:IRepository<Event>
{
    Task<List<Event>> GetUpcomingEventsWithCategoryAsync();
    Task<List<Event>> GetEventsByCategoryIdAsync(int categoryId);
    Task<List<Event>> SearchEventsAsync(string searchText);
    Task<Event?> GetEventDetailAsync(int id);
    Task<Event?> GetFeaturedEventAsync();
}