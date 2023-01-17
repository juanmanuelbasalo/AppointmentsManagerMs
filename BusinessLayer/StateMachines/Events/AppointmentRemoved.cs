using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public record AppointmentRemoved : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; init; }
        public Guid CorrelationId => AppointmentId;
    }
}
