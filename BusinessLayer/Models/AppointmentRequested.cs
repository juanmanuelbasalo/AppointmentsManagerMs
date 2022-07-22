using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentRequested : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentStartsAt { get; set; }
        public TimeSpan AppointmentDuration { get; set; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }
        public Guid DoctorOfficeId { get; set; }
        public AppointmentUser Client { get; set; } = new AppointmentUser();

        public Guid CorrelationId => AppointmentId;
    }
}
