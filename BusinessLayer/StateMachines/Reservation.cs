using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class Reservation : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeOnly EndsAt { get => StartsAt.Add(Duration); }
        public bool IsReserved { get; set; }
        public Guid? TokenExpirationId { get; set; }
        public Guid DoctorOfficeId { get; set; }
        public AppointmentUser Client { get; set; } = new AppointmentUser();
    }
}
