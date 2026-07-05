using Application.Abstract.Service;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Home;

namespace Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IEventService _eventService;

    public HomeController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? searchText)
    {
        var homePage = await _eventService.GetHomePageAsync();

        if (categoryId.HasValue)
        {
            homePage.Events = await _eventService.GetEventsByCategoryAsync(categoryId.Value);
        }

        if (searchText != null && searchText != "")
        {
            homePage.Events = await _eventService.SearchEventsAsync(searchText);
        }

        var model = new HomeIndexViewModel
        {
            Categories = homePage.Categories,
            Events = homePage.Events,
            FeaturedEvent = homePage.FeaturedEvent,
            SelectedCategoryId = categoryId,
            SearchText = searchText
        };

        return View(model);
    }
}
