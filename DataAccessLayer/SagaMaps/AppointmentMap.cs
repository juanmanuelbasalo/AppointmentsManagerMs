using AppointmentsManagerMs.BusinessLayer.StateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsManagerMs.DataAccessLayer.SagaMaps
{
    public class AppointmentMap : SagaClassMap<Appointment>
    {
        protected override void Configure(EntityTypeBuilder<Appointment> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState)
                .HasMaxLength(64);

            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
