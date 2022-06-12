namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class AppointmentEntity : BaseEntity
    {
        public AppointmentEntity() : base(Guid.NewGuid())
        {
        }

        public TimeSpan AppointmentDuration { get; set; }
        public TimeOnly AppointmentStartsAt { get; set; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }
        public DateOnly AppointmentDate { get; set; }
        public string ClientFullname { get => $"{ClientFirstName} {ClientLastName}"; }
        public string ClientFirstName { get; set; } = "";
        public string ClientLastName { get; set; } = "";
        public string ClientEmail { get; set; } = "";
        public string ClientPhone { get; set; } = "";
        public Guid DoctorOfficeId { get; set; }
        public DoctorOfficeEntity DoctorOffice { get; set; } = new DoctorOfficeEntity(Guid.Empty);
    }
}
