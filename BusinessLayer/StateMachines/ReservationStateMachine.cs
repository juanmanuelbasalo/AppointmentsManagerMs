using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class ReservationStateMachine : MassTransitStateMachine<Reservation>
    {
        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Requested, Reserved);

            Schedule(() => ExpirationSchedule, x => x.TokenExpirationId);

            Initially(
                When(AppointmentRequested)
                    .Then(context =>
                    {
                        context.Saga.Date = context.Message.AppointmentDate;
                        context.Saga.StartsAt = context.Message.AppointmentStartsAt;
                        context.Saga.Duration = context.Message.AppointmentDuration;
                        context.Saga.DoctorOfficeId = context.Message.DoctorOfficeId;
                    })
                    .TransitionTo(Requested),
                When(AppointmentReservationExpired)
                    .Finalize()
            );

            During(Requested,
                When(AppointmentReserved)
                    .Then(context => context.Saga.IsReserved = true)
                    .Schedule(ExpirationSchedule, context => context.Init<AppointmentReservationExpired>(new { AppointmentId = context.Saga.CorrelationId }),
                               context => context.Saga.Date.ToDateTime(context.Saga.EndsAt))
                    .TransitionTo(Reserved));

            During(Reserved,
                When(AppointmentReservationExpired)
                    .PublishAsync(context => context.Init<AppointmentExpired>(new
                    {
                        context.Message.AppointmentId
                    }))
                    .Finalize(),
                When(AppointmentReservationCanceled)
                    .Unschedule(ExpirationSchedule)
                    .PublishAsync(context => context.Init<AppointmentExpired>(new
                    {
                        context.Message.AppointmentId
                    }))
                    .Finalize()
             );

            SetCompletedWhenFinalized();
        }

        public State Requested { get; }
        public State Reserved { get; }

        public Schedule<Reservation, AppointmentReservationExpired> ExpirationSchedule { get; set; }

        public Event<AppointmentRequested> AppointmentRequested { get; }
        public Event<AppointmentReserved> AppointmentReserved { get; }
        public Event<AppointmentReservationExpired> AppointmentReservationExpired { get; }
        public Event<AppointmentReservationCanceled> AppointmentReservationCanceled { get; }
    }
}
