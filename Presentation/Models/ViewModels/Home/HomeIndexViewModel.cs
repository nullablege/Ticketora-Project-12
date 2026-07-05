using Application.DTOs.Common;
using Application.DTOs.Events;

namespace Presentation.Models.ViewModels.Home;

public class HomeIndexViewModel
{
    public List<CategoryDto> Categories { get; set; }
    public List<EventCardDto> Events { get; set; }
    public EventCardDto? FeaturedEvent { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string? SearchText { get; set; }
}
