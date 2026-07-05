namespace Domain.Entities;

public class EventRegistration:BaseEntity
{
    public int UserId { get; set; }
    
    public Event Event { get; set; }
    public int EventId { get; set; }

    public List<Ticket> Tickets { get; set; } = new(); 
    
    public decimal TotalPrice { get; set; }
}