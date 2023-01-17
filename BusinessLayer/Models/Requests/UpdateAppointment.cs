namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public record UpdateAppointment
    {
        public DateTime Date { get; init; }
        public TimeSpan Duration { get; init; }
        public Guid DoctorOfficeId { get; init; }
    }
}