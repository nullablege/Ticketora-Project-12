using Application.DTOs.Common;

namespace Application.DTOs.Events;

public class HomePageDto
{
    public List<CategoryDto> Categories { get; set; } = new();
    public List<EventCardDto> Events { get; set; } = new();
    public EventCardDto? FeaturedEvent { get; set; }
}
