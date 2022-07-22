using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentReservationExpired : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid CorrelationId => AppointmentId;
    }
}
