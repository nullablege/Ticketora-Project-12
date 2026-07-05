namespace Application.DTOs.Events;

public class EventCardDto
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string HeroImage { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string TimeRange { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public bool IsFeatured { get; set; }
}
