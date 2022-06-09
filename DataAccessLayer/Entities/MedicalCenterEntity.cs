namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class MedicalCenterEntity : BaseEntity
    {
        protected MedicalCenterEntity(Guid id) : base(id)
        {
        }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
    }
}
