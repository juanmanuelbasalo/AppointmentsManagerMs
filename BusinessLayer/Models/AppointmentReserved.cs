using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentReserved : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; }
        public DateOnly AppointmentDate { get; }
        public TimeOnly AppointmentStartsAt { get; }
        public Guid DoctorOfficeId { get; }
        public AppointmentUser Client { get; }

        public Guid CorrelationId => AppointmentId;
    }
}
