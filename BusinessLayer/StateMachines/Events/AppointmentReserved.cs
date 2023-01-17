using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.Events
{
    public class AppointmentReserved : CorrelatedBy<Guid>
    {
        public Guid AppointmentId { get; init; }

        public Guid CorrelationId => AppointmentId;
    }
}
