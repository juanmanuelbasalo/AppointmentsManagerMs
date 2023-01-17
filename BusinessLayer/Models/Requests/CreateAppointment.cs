namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public record CreateAppointment
    {
        public DateTime Date { get; init; }
        public TimeSpan Duration { get; init; }
        public Guid DoctorOfficeId { get; init; }
        public Guid DoctorId { get; init; }
    }
}