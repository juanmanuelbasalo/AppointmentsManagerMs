using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentAdded : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid CorrelationId => AppointmentId;
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeOnly EndsAt { get => StartsAt.Add(Duration); }
        public Guid DoctorOfficeId { get; set; }
    }
}
