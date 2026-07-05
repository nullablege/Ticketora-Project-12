using Application.Abstract.Repository;
using Application.Abstract.Service;
using Application.DTOs.Dashboard;
using Domain.Entities;

namespace Application.Services;

public class DashboardService:IDashboardService
{
    private readonly IEventRegistrationRepository _eventRegistrationRepository;
    private readonly ITicketRepository _ticketRepository;

    public DashboardService(
        IEventRegistrationRepository eventRegistrationRepository,
        ITicketRepository ticketRepository)
    {
        _eventRegistrationRepository = eventRegistrationRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(int userId)
    {
        var activeTicketCount = await _ticketRepository.GetActiveTicketCountByUserIdAsync(userId);
        var pastEvents = await _eventRegistrationRepository.GetPastRegistrationsByUserIdAsync(userId);
        var recentTickets = await _ticketRepository.GetRecentActiveTicketsByUserIdAsync(userId, 2);

        return new DashboardSummaryDto
        {
            ActiveTicketCount = activeTicketCount,
            PastEventCount = pastEvents.Count,
            RecentActiveTickets = recentTickets.Select(MapTicketCard).ToList()
        };
    }

    public async Task<List<TicketCardDto>> GetActiveTicketsAsync(int userId)
    {
        var tickets = await _ticketRepository.GetActiveTicketsByUserIdAsync(userId);

        return tickets.Select(MapTicketCard).ToList();
    }

    public async Task<List<PastEventDto>> GetPastEventsAsync(int userId)
    {
        var registrations = await _eventRegistrationRepository.GetPastRegistrationsByUserIdAsync(userId);

        return registrations.Select(x => new PastEventDto
        {
            RegistrationId = x.Id,
            EventId = x.EventId,
            EventTitle = x.Event.Title,
            EventImage = x.Event.HeroImage,
            EventDate = x.Event.Date,
            EventLocation = x.Event.Location,
            TotalPrice = x.TotalPrice
        }).ToList();
    }

    public async Task<TicketDetailDto?> GetTicketDetailAsync(int ticketId, int userId)
    {
        var ticket = await _ticketRepository.GetTicketDetailByIdAsync(ticketId, userId);

        return ticket == null ? null : MapTicketDetail(ticket);
    }

    private static TicketCardDto MapTicketCard(Ticket ticket)
    {
        var eventItem = ticket.EventRegistration.Event;

        return new TicketCardDto
        {
            TicketId = ticket.Id,
            EventTitle = eventItem.Title,
            EventImage = eventItem.HeroImage,
            EventDate = eventItem.Date,
            EventTimeRange = eventItem.TimeRange,
            EventLocation = eventItem.Location,
            SerialNo = ticket.SerialNo,
            Seat = ticket.Seat
        };
    }

    private static TicketDetailDto MapTicketDetail(Ticket ticket)
    {
        var eventItem = ticket.EventRegistration.Event;

        return new TicketDetailDto
        {
            TicketId = ticket.Id,
            EventTitle = eventItem.Title,
            EventDate = eventItem.Date,
            EventTimeRange = eventItem.TimeRange,
            EventLocation = eventItem.Location,
            PassengerFullName = ticket.PassengerFullName,
            PassengerEmail = ticket.PassengerEmail,
            SerialNo = ticket.SerialNo,
            Seat = ticket.Seat
        };
    }
}
