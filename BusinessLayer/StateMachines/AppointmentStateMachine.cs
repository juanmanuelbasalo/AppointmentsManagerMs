using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.StateMachines.CustomActivities;
using AppointmentsManagerMs.BusinessLayer.StateMachines.Events;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class AppointmentStateMachine : MassTransitStateMachine<Appointment>
    {
        public AppointmentStateMachine()
        {
            InstanceState(x => x.CurrentState, Available, Reserved, Removed);

            Initially(
                When(AppointmentAdded)
                    .Then(context =>
                    {
                        context.Saga.AppointmentDate = context.Message.Date;
                        context.Saga.AppointmentStartsAt = context.Message.StartsAt;
                        context.Saga.AppointmentDuration = context.Message.Duration;
                        context.Saga.DoctorOfficeId = context.Message.DoctorOfficeId;
                        context.Saga.DoctorId = context.Message.DoctorId;
                    })
                    .TransitionTo(Available));

            During(Available,
                When(AppointmentReserved)
                    .TransitionTo(Reserved),
                When(AppointmentRemoved)
                    .Then(context =>
                    {
                        context.Saga.RemovedAt = InVar.Timestamp;
                    })
                    .TransitionTo(Removed),
                When(AppointmentUpdated)
                    .Activity(act => act.OfType<UpdateAppointmentActivity>()));


            During(Reserved,
                When(AppointmentExpired)
                    .Finalize(),
                When(AppointmentRemoved)
                    .Then(context =>
                    {
                        context.Saga.RemovedAt = InVar.Timestamp;
                    })
                    .TransitionTo(Removed),
                When(AppointmentCanceled)
                    .TransitionTo(Available),
                Ignore(AppointmentUpdated));

            During(Removed, 
                Ignore(AppointmentReserved),
                Ignore(AppointmentAdded),
                Ignore(AppointmentUpdated),
                Ignore(AppointmentCanceled));
        }

        public State Available { get; }
        public State Reserved { get; }
        public State Removed { get; }

        public Event<AppointmentAdded> AppointmentAdded { get; }
        public Event<AppointmentReserved> AppointmentReserved { get; }
        public Event<AppointmentExpired> AppointmentExpired { get; }
        public Event<AppointmentRemoved> AppointmentRemoved { get; }
        public Event<AppointmentCanceled> AppointmentCanceled { get; }
        public Event<AppointmentUpdated> AppointmentUpdated { get; }
    }
}
