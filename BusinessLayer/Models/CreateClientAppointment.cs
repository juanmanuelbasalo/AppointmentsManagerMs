namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class CreateClientAppointment
    {
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentStartsAt { get; set; }
        public TimeSpan AppointmentDuration { get; set; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }
    }
}
