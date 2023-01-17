using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public record AppointmentUpdated : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; init; }
        public Guid CorrelationId => AppointmentId;
        public DateOnly Date { get; init; }
        public TimeOnly StartsAt { get; init; }
        public TimeSpan Duration { get; init; }
        public TimeOnly EndsAt { get => StartsAt.Add(Duration); }
        public Guid DoctorOfficeId { get; init; }
    }
}
