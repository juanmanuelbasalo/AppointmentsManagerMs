namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class AppointmentEntity : BaseEntity
    {
        protected AppointmentEntity() : base(Guid.NewGuid())
        {
        }
    }
}
