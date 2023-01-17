using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public class AppointmentCanceled : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; }
        public Guid CorrelationId => AppointmentId;
    }
}
