using Application.DTOs.Booking;

namespace Presentation.Models.ViewModels.Booking;

public class CheckoutViewModel
{
    public CheckoutDto Checkout { get; set; }
    public CreateBookingRequest Request { get; set; }
}
