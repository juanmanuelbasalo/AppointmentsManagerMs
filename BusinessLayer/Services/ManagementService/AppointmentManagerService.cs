using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.BusinessLayer.Utils.AppointmentsValidation;
using AppointmentsManagerMs.DataAccessLayer;
using AppointmentsManagerMs.DataAccessLayer.Entities;
using AppointmentsManagerMs.DataAccessLayer.Repository;

namespace AppointmentsManagerMs.BusinessLayer.Services.ManagementService
{
    public class AppointmentManagerService : IAppointmentManagerService
    {
        private readonly IGenericRepository<SagaAppointmentsManagerContext> _repo;
        public AppointmentManagerService(IGenericRepository<SagaAppointmentsManagerContext> genericRepo)
        {
            _repo = genericRepo;
        }

        public bool IsTimeSlotTaken(DateOnly appointmentDate, TimeOnly requestedAppointmentStartsAt, TimeOnly requestedAppointmentEndsAt)
        {
            var appointments = _repo.GetAllReadOnly<Appointment>(apt => apt.AppointmentDate == appointmentDate).ToArray();
            var isAlreadyTaken = appointments.Any(apt => AppointmentConstraints.IsNotValidAppointmentInterval(requestedAppointmentStartsAt, apt.AppointmentEndsAt,
                requestedAppointmentEndsAt, apt.AppointmentStartsAt));

            return isAlreadyTaken;
        }
    }
}
