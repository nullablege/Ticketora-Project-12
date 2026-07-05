namespace Domain.Entities;

public class Ticket:BaseEntity
{
    public int EventRegistrationId { get; set; }
    public EventRegistration EventRegistration { get; set; }
    
    public string PassengerFullName { get; set; }
    public string PassengerEmail { get; set; }
    
    public string SerialNo { get; set; }
    public string Seat  { get; set; }
}