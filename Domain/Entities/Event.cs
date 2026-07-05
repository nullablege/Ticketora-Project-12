namespace Domain.Entities;

public class Event:BaseEntity
{
    public int CategoryId { get; set; } 
    public Category Category { get; set; }
    
    public string Title { get; set; }
    public string HeroImage { get; set; }
    public string Organizer  { get; set; }
    public string Description { get; set; }
    public string Badges  { get; set; }
    public DateTime Date { get; set; }
    public string TimeRange  { get; set; }
    public string Location  { get; set; }
    public decimal Price { get; set; }
    public bool IsFeatured  { get; set; }
    

    public List<EventRegistration> Registrations { get; set; } = new();

}