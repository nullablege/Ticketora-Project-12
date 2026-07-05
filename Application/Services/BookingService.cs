using Application.Abstract.Repository;
using Application.Abstract.Service;
using Application.DTOs.Booking;
using Domain.Entities;

namespace Application.Services;

public class BookingService:IBookingService
{
    private readonly IEventRegistrationRepository _eventRegistrationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ITicketRepository _ticketRepository;

    public BookingService(
        IEventRegistrationRepository eventRegistrationRepository,
        IEventRepository eventRepository,
        ITicketRepository ticketRepository)
    {
        _eventRegistrationRepository = eventRegistrationRepository;
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<CheckoutDto?> GetCheckoutAsync(int eventId)
    {
        var eventDetail = await _eventRepository.GetEventDetailAsync(eventId);

        if (eventDetail == null)
        {
            return null;
        }

        return new CheckoutDto
        {
            EventId = eventDetail.Id,
            EventTitle = eventDetail.Title,
            EventImage = eventDetail.HeroImage,
            EventDate = eventDetail.Date,
            EventTimeRange = eventDetail.TimeRange,
            EventLocation = eventDetail.Location,
            Price = eventDetail.Price
        };
    }

    public async Task<BookingResultDto> CompleteBookingAsync(CreateBookingRequest request)
    {
        var eventDetail = await _eventRepository.GetEventDetailAsync(request.EventId);

        if (eventDetail == null)
        {
            throw new InvalidOperationException("Etkinlik bulunamadı.");
        }

        var registration = new EventRegistration
        {
            UserId = request.UserId,
            EventId = eventDetail.Id,
            TotalPrice = eventDetail.Price
        };

        var ticket = new Ticket
        {
            PassengerFullName = request.PassengerFullName,
            PassengerEmail = request.PassengerEmail,
            SerialNo = GenerateSerialNo(),
            Seat = request.Seat == null ? "Genel Giriş" : request.Seat
        };

        var createdRegistration = await _eventRegistrationRepository
            .CreateRegistrationWithTicketAsync(registration, ticket);

        return new BookingResultDto
        {
            RegistrationId = createdRegistration.Id,
            TicketId = ticket.Id,
            EventTitle = eventDetail.Title,
            EventDate = eventDetail.Date,
            EventTimeRange = eventDetail.TimeRange,
            EventLocation = eventDetail.Location,
            PassengerFullName = ticket.PassengerFullName,
            PassengerEmail = ticket.PassengerEmail,
            SerialNo = ticket.SerialNo,
            Seat = ticket.Seat,
            TotalPrice = createdRegistration.TotalPrice
        };
    }

    public async Task<BookingResultDto?> GetBookingResultAsync(int ticketId, int userId)
    {
        var ticket = await _ticketRepository.GetTicketDetailByIdAsync(ticketId, userId);

        if (ticket == null)
        {
            return null;
        }

        var eventItem = ticket.EventRegistration.Event;

        return new BookingResultDto
        {
            RegistrationId = ticket.EventRegistrationId,
            TicketId = ticket.Id,
            EventTitle = eventItem.Title,
            EventDate = eventItem.Date,
            EventTimeRange = eventItem.TimeRange,
            EventLocation = eventItem.Location,
            PassengerFullName = ticket.PassengerFullName,
            PassengerEmail = ticket.PassengerEmail,
            SerialNo = ticket.SerialNo,
            Seat = ticket.Seat,
            TotalPrice = ticket.EventRegistration.TotalPrice
        };
    }

    private static string GenerateSerialNo()
    {
        return $"TK-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}";
    }
}
