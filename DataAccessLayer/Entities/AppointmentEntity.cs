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
        public DoctorOfficeEntity DoctorOffice { get; set; }
    }
}
