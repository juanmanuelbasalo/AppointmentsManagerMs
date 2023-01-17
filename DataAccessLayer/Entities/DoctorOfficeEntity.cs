namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class DoctorEntity : BaseEntity
    {
        public DoctorEntity(Guid id) : base(id)
        {
        }
        public string DoctorName { get; set; }
        public string DoctorDescription { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorEmail { get; set; }
    }
}
