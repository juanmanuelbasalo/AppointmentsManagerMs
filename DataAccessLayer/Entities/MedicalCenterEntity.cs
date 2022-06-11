namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class MedicalCenterEntity : BaseEntity
    {
        public MedicalCenterEntity(Guid id) : base(id)
        {
        }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public ICollection<DoctorOfficeEntity> DoctorOffices { get; set; }
    }
}
