using Application.DTOs.Booking;

namespace Application.Abstract.Service;

public interface IBookingService
{
    Task<CheckoutDto?> GetCheckoutAsync(int eventId);
    Task<BookingResultDto> CompleteBookingAsync(CreateBookingRequest request);
    Task<BookingResultDto?> GetBookingResultAsync(int ticketId, int userId);
}
