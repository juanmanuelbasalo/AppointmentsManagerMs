using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public class ReservationCanceled : CorrelatedBy<Guid>
    {
        public Guid ReservationId { get; set; }
        public Guid CorrelationId => ReservationId;
    }
}
