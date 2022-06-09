namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class DoctorOfficeEntity : BaseEntity
    {
        protected DoctorOfficeEntity(Guid id) : base(id)
        {
        }
        public string DoctorName { get; set; }
        public string DoctorDescription { get; set; }
        public string OfficeNumber { get; set; }
        public string OfficeDescription { get; set; }
    }
}
