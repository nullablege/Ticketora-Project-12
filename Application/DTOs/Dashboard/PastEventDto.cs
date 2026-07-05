namespace Application.DTOs.Dashboard;

public class PastEventDto
{
    public int RegistrationId { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; }
    public string EventImage { get; set; }
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; }
    public decimal TotalPrice { get; set; }
}
