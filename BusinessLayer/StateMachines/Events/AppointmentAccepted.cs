using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public record AppointmentAccepted : CorrelatedBy<Guid>
    {
        public Guid ReservationId { get; init; }

        public Guid CorrelationId => ReservationId;
    }
}
