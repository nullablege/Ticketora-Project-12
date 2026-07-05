using Application.Abstract.Repository;
using Application.Abstract.Service;
using Application.DTOs.Common;
using Application.DTOs.Events;
using Domain.Entities;

namespace Application.Services;

public class EventService:IEventService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventRepository _eventRepository;

    public EventService(ICategoryRepository categoryRepository, IEventRepository eventRepository)
    {
        _categoryRepository = categoryRepository;
        _eventRepository = eventRepository;
    }

    public async Task<HomePageDto> GetHomePageAsync()
    {
        var categories = await _categoryRepository.GetActiveCategoriesAsync();
        var events = await _eventRepository.GetUpcomingEventsWithCategoryAsync();
        var featuredEvent = await _eventRepository.GetFeaturedEventAsync();

        return new HomePageDto
        {
            Categories = categories.Select(MapCategory).ToList(),
            Events = events.Select(MapEventCard).ToList(),
            FeaturedEvent = featuredEvent == null ? events.Select(MapEventCard).FirstOrDefault() : MapEventCard(featuredEvent)
        };
    }

    public async Task<List<EventCardDto>> GetEventsByCategoryAsync(int categoryId)
    {
        var events = await _eventRepository.GetEventsByCategoryIdAsync(categoryId);

        return events.Select(MapEventCard).ToList();
    }

    public async Task<List<EventCardDto>> SearchEventsAsync(string searchText)
    {
        var events = await _eventRepository.SearchEventsAsync(searchText);

        return events.Select(MapEventCard).ToList();
    }

    public async Task<EventDetailDto?> GetEventDetailAsync(int id)
    {
        var eventDetail = await _eventRepository.GetEventDetailAsync(id);

        return eventDetail == null ? null : MapEventDetail(eventDetail);
    }

    private static CategoryDto MapCategory(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            CategoryName = category.CategoryName
        };
    }

    private static EventCardDto MapEventCard(Event eventItem)
    {
        return new EventCardDto
        {
            Id = eventItem.Id,
            CategoryName = eventItem.Category.CategoryName,
            Title = eventItem.Title,
            HeroImage = eventItem.HeroImage,
            Description = eventItem.Description,
            Date = eventItem.Date,
            TimeRange = eventItem.TimeRange,
            Location = eventItem.Location,
            Price = eventItem.Price,
            IsFeatured = eventItem.IsFeatured
        };
    }

    private static EventDetailDto MapEventDetail(Event eventItem)
    {
        return new EventDetailDto
        {
            Id = eventItem.Id,
            CategoryId = eventItem.CategoryId,
            CategoryName = eventItem.Category.CategoryName,
            Title = eventItem.Title,
            HeroImage = eventItem.HeroImage,
            Organizer = eventItem.Organizer,
            Description = eventItem.Description,
            Badges = eventItem.Badges,
            Date = eventItem.Date,
            TimeRange = eventItem.TimeRange,
            Location = eventItem.Location,
            Price = eventItem.Price,
            IsFeatured = eventItem.IsFeatured
        };
    }
}
