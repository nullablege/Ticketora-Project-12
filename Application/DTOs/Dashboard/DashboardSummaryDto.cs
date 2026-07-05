namespace Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int ActiveTicketCount { get; set; }
    public int PastEventCount { get; set; }
    public List<TicketCardDto> RecentActiveTickets { get; set; } = new();
}
