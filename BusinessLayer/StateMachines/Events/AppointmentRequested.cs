using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public record AppointmentRequested : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; init; }
        public Guid ReservationId { get; init; }
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeOnly EndsAt { get => StartsAt.Add(Duration); }
        public AppointmentUser Client { get; init; } = new AppointmentUser();

        public Guid CorrelationId => ReservationId;
    }
}
