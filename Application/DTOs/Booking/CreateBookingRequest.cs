namespace Application.DTOs.Booking;

public class CreateBookingRequest
{
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string PassengerFullName { get; set; }
    public string PassengerEmail { get; set; }
    public string? Seat { get; set; }
}
