using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.DataAccessLayer.Entities;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public class AppointmentUser : BaseEntity
    {
        public AppointmentUser() : base(NewId.NextGuid())
        {
        }

        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? DocumentId { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
