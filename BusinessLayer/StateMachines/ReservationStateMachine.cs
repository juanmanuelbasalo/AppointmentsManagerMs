using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.StateMachines.Events;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class ReservationStateMachine : MassTransitStateMachine<Reservation>
    {
        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Requested, Accepted);

            Schedule(() => ExpirationSchedule, x => x.TokenExpirationId);

            Initially(
                When(AppointmentRequested)
                    .Then(context =>
                    {
                        context.Saga.AppointmentId = context.Message.AppointmentId;
                        context.Saga.Client = context.Message.Client;
                        context.Saga.Date = context.Message.Date;
                        context.Saga.StartsAt = context.Message.StartsAt;
                        context.Saga.Duration = context.Message.Duration;
                    })
                    .TransitionTo(Requested),
                When(AppointmentReservationExpired)
                    .Finalize()
            );

            During(Requested,
                When(AppointmentAccepted)
                    .Then(context => context.Saga.IsReserved = true)
                    //.Schedule(ExpirationSchedule, context => context.Init<ReservationExpired>(new { context.Saga.AppointmentId }),
                    //           context => context.Saga.Date.ToDateTime(context.Saga.EndsAt))
                    .Publish(context => new AppointmentReserved { AppointmentId = context.Saga.AppointmentId })
                    .TransitionTo(Accepted),
                When(AppointmentRemoved)

                    .Finalize()
            );

            During(Accepted,
                When(AppointmentReservationExpired)
                    .PublishAsync(context => context.Init<AppointmentExpired>(new
                    {
                        context.Message.AppointmentId
                    }))
                    .Finalize(),
                When(AppointmentReservationCanceled)    
                    .Unschedule(ExpirationSchedule)
                    .PublishAsync(context => context.Init<AppointmentCanceled>(new
                    {
                        context.Saga.AppointmentId
                    }))
                    .Finalize(),
                When(AppointmentRemoved)
                    .Finalize()
             );

            SetCompletedWhenFinalized();
        }

        public State Requested { get; }
        public State Accepted { get; }

        public Schedule<Reservation, ReservationExpired> ExpirationSchedule { get; set; }

        public Event<AppointmentRequested> AppointmentRequested { get; }
        public Event<AppointmentAccepted> AppointmentAccepted { get; }
        public Event<ReservationExpired> AppointmentReservationExpired { get; }
        public Event<ReservationCanceled> AppointmentReservationCanceled { get; }
        public Event<AppointmentRemoved> AppointmentRemoved { get; }
    }
}
