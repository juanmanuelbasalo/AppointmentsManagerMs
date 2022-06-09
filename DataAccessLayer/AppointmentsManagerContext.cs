using AppointmentsManagerMs.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsManagerMs.DataAccessLayer
{
    public class AppointmentsManagerContext : DbContext
    {
        public AppointmentsManagerContext(DbContextOptions<AppointmentsManagerContext> options) : base(options)
        {
        }

        public DbSet<AppointmentEntity> Appointments { get; set; }
        public DbSet<MedicalCenterEntity> MedicalCenters { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<DoctorOfficeEntity> DoctorsOffices { get; set; }

        public async Task<bool> SaveChangesAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving(userEmail);
            return await (SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken)) > 0;
        }

        protected virtual void OnBeforeSaving(string userEmail)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow.UtcDateTime;
                        entry.Entity.CreatedBy = userEmail;
                        break;

                    // Write modification date
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow.UtcDateTime;
                        entry.Entity.UpdatedBy = userEmail;
                        break;
                }
        }
    }
}
