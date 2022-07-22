using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentReservationCanceled : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid CorrelationId => AppointmentId;
    }
}
