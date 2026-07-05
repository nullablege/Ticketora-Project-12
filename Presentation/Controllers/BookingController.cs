using Application.Abstract.Service;
using Application.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Booking;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IActionResult> Checkout(int id = 1)
    {
        var checkout = await _bookingService.GetCheckoutAsync(id);

        if (checkout == null)
        {
            return NotFound();
        }

        var model = new CheckoutViewModel
        {
            Checkout = checkout,
            Request = new CreateBookingRequest
            {
                EventId = checkout.EventId,
                UserId = GetCurrentUserId(),
                PassengerFullName = GetCurrentUserName(),
                PassengerEmail = GetCurrentUserEmail()
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CreateBookingRequest request)
    {
        request.UserId = GetCurrentUserId();

        var ticket = await _bookingService.CompleteBookingAsync(request);

        return RedirectToAction("Success", new { ticketId = ticket.TicketId });
    }

    public async Task<IActionResult> Success(int ticketId = 0)
    {
        if (ticketId == 0)
        {
            return RedirectToAction("Index", "Home");
        }

        var ticket = await _bookingService.GetBookingResultAsync(ticketId, GetCurrentUserId());

        if (ticket == null)
        {
            return NotFound();
        }

        var model = new BookingSuccessViewModel
        {
            Ticket = ticket
        };

        return View(model);
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        int.TryParse(userId, out int id);

        return id;
    }

    private string GetCurrentUserName()
    {
        var name = User.Identity?.Name;

        if (name == null)
        {
            return "";
        }

        return name;
    }

    private string GetCurrentUserEmail()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
        {
            return GetCurrentUserName();
        }

        return email;
    }
}
