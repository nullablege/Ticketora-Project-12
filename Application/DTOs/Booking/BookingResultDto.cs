namespace Application.DTOs.Booking;

public class BookingResultDto
{
    public int RegistrationId { get; set; }
    public int TicketId { get; set; }
    public string EventTitle { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTimeRange { get; set; }
    public string EventLocation { get; set; }
    public string PassengerFullName { get; set; }
    public string PassengerEmail { get; set; }
    public string SerialNo { get; set; }
    public string Seat { get; set; }
    public decimal TotalPrice { get; set; }
}
