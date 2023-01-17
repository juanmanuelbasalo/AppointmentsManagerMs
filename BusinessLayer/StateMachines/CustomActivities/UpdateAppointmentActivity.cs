using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.StateMachines.Events;
using AppointmentsManagerMs.BusinessLayer.Utils.AppointmentsValidation;
using AppointmentsManagerMs.DataAccessLayer;
using AppointmentsManagerMs.DataAccessLayer.Repository;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines.CustomActivities
{
    public class UpdateAppointmentActivity : IStateMachineActivity<Appointment, AppointmentUpdated>
    {
        private readonly IGenericRepository<SagaAppointmentsManagerContext> _repository;
        public UpdateAppointmentActivity(IGenericRepository<SagaAppointmentsManagerContext> repository)
        {
            _repository = repository;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("updateAppointment");
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<Appointment, AppointmentUpdated> context, IBehavior<Appointment, AppointmentUpdated> next)
        {
            var message = context.Message;
            var instance = context.Saga;

            var appointments = _repository.GetAllReadOnly<Appointment>(apt => apt.AppointmentDate == message.Date);
            var isAlreadyTaken = appointments.Any(apt => AppointmentConstraints.IsNotValidAppointmentInterval(message.StartsAt, apt.AppointmentEndsAt,
                message.EndsAt, apt.AppointmentStartsAt));

            if (isAlreadyTaken)
                throw new Exception("This time slot is already booked by other appointment");

            instance.AppointmentDate = message.Date;
            instance.AppointmentStartsAt = message.StartsAt;
            instance.AppointmentDuration = message.Duration;
            instance.DoctorOfficeId = message.DoctorOfficeId;

            await _repository.SaveAsync("");

            await next.Execute(context);
        }

        public async Task Faulted<TException>(BehaviorExceptionContext<Appointment, AppointmentUpdated, TException> context, 
            IBehavior<Appointment, AppointmentUpdated> next) where TException : Exception
        {
            await next.Faulted(context);
        }
    }
}
