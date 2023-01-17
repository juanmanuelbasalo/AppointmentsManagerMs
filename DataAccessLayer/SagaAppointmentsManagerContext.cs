using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.DataAccessLayer.SagaMaps;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsManagerMs.DataAccessLayer
{
    public class SagaAppointmentsManagerContext : SagaDbContext
    {
        public SagaAppointmentsManagerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppointmentUser> Clients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new AppointmentMap();
                yield return new ReservationMap();
            }
        }
    }
}
