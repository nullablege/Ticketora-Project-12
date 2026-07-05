using Application.DTOs.Dashboard;

namespace Application.Abstract.Service;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(int userId);
    Task<List<TicketCardDto>> GetActiveTicketsAsync(int userId);
    Task<List<PastEventDto>> GetPastEventsAsync(int userId);
    Task<TicketDetailDto?> GetTicketDetailAsync(int ticketId, int userId);
}
