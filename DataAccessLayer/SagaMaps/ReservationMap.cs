using AppointmentsManagerMs.BusinessLayer.StateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentsManagerMs.DataAccessLayer.SagaMaps
{
    public class ReservationMap : SagaClassMap<Reservation>
    {
        protected override void Configure(EntityTypeBuilder<Reservation> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState)
                .HasMaxLength(64);

            entity.HasOne(x => x.Client);

            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
