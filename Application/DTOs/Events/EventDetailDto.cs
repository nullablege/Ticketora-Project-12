namespace Application.DTOs.Events;

public class EventDetailDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string HeroImage { get; set; }
    public string Organizer { get; set; }
    public string Description { get; set; }
    public string Badges { get; set; }
    public DateTime Date { get; set; }
    public string TimeRange { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public bool IsFeatured { get; set; }
}
