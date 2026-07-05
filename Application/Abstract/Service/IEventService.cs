using Application.DTOs.Events;

namespace Application.Abstract.Service;

public interface IEventService
{
    Task<HomePageDto> GetHomePageAsync();
    Task<List<EventCardDto>> GetEventsByCategoryAsync(int categoryId);
    Task<List<EventCardDto>> SearchEventsAsync(string searchText);
    Task<EventDetailDto?> GetEventDetailAsync(int id);
}
