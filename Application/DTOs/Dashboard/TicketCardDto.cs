namespace Application.DTOs.Dashboard;

public class TicketCardDto
{
    public int TicketId { get; set; }
    public string EventTitle { get; set; }
    public string EventImage { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTimeRange { get; set; }
    public string EventLocation { get; set; }
    public string SerialNo { get; set; }
    public string Seat { get; set; }
}
