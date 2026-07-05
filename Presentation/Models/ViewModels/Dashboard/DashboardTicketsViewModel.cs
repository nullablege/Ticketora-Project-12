using Application.DTOs.Dashboard;

namespace Presentation.Models.ViewModels.Dashboard;

public class DashboardTicketsViewModel
{
    public List<TicketCardDto> ActiveTickets { get; set; }
    public TicketDetailDto? SelectedTicket { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}
