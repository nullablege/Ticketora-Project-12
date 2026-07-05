using Application.Abstract.Service;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Events;

namespace Presentation.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IActionResult> Detail(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        id = id.Replace("ev-", "");

        var result = int.TryParse(id, out int eventId);

        if (!result)
        {
            return NotFound();
        }

        var eventDetail = await _eventService.GetEventDetailAsync(eventId);

        if (eventDetail == null)
        {
            return NotFound();
        }

        var model = new EventDetailViewModel
        {
            Event = eventDetail
        };

        return View(model);
    }
}
