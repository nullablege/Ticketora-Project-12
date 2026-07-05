namespace Domain.Entities;

public class Category:BaseEntity
{
    public string CategoryName { get; set; }

    public List<Event> Events { get; set; } = new();
}