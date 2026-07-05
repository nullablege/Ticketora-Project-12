namespace Application.DTOs.Booking;

public class CheckoutDto
{
    public int EventId { get; set; }
    public string EventTitle { get; set; }
    public string EventImage { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTimeRange { get; set; }
    public string EventLocation { get; set; }
    public decimal Price { get; set; }
}
