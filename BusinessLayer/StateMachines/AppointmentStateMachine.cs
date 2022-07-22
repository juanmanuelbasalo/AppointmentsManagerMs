using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class AppointmentStateMachine : MassTransitStateMachine<Appointment>
    {
        public AppointmentStateMachine()
        {
            InstanceState(x => x.CurrentState, Available, Accepted);

            Initially(
                When(AppointmentAdded)
                .Then(context =>
                {
                    context.Saga.AppointmentDate = context.Message.Date;
                    context.Saga.AppointmentStartsAt = context.Message.StartsAt;
                    context.Saga.AppointmentDuration = context.Message.Duration;
                    context.Saga.DoctorOfficeId = context.Message.DoctorOfficeId;
                })
                .TransitionTo(Available));

            During(Available,
                When(AppointmentRequested)
                .TransitionTo(Accepted)
                .PublishAsync(context => context.Init<AppointmentReserved>(new 
                {
                    context.Message.AppointmentId,
                    context.Message.AppointmentDate,
                    context.Message.AppointmentStartsAt,
                    context.Message.DoctorOfficeId,
                    context.Message.Client
                })));

            During(Accepted,
                When(AppointmentExpired)
                    .Finalize());

            SetCompletedWhenFinalized();
        }

        public State Available { get; }
        public State Accepted { get; }

        public Event<AppointmentAdded> AppointmentAdded { get; }
        public Event<AppointmentRequested> AppointmentRequested { get; }
        public Event<AppointmentExpired> AppointmentExpired { get; }
    }
}
