namespace AppointmentsManagerMs.DataAccessLayer.Entities
{
    public class ClientEntity : BaseEntity
    {
        protected ClientEntity(Guid id) : base(id)
        {
        }

        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string FullName { get => $"{Name} {LastName}"; }
        public string? Email { get; set; }
        public string? ContactPhone { get; set; }
    }
}
