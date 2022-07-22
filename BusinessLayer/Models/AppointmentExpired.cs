using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentExpired : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid CorrelationId => AppointmentId;
    }
}
