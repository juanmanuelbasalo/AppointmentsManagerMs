namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class CreateClientAppointment
    {
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentStartsAt { get; set; }
        public TimeSpan AppointmentDuration { get; set; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }
        public Guid DoctorOfficeId { get; set; }
        public ClientAppointmentUser Client { get; set; } = new ClientAppointmentUser();
    }

    public class ClientAppointmentUser
    {
        public string Name { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string UserPhone { get; set; } = "";
    }
}
