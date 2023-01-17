using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public class AppointmentExpired : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid CorrelationId => AppointmentId;
    }
}
