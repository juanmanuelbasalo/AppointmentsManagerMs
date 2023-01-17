using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public class ReservationExpired : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid ReservationId { get; set; }
        public Guid CorrelationId => ReservationId;
    }
}
