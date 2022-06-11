using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.DataAccessLayer.Entities;
using AppointmentsManagerMs.DataAccessLayer.Repository;

namespace AppointmentsManagerMs.BusinessLayer.Services.ManagementService
{
    public class AppointmentManagerService : IAppointmentManagerService
    {
        private readonly IGenericRepository _genericRepo;
        public AppointmentManagerService(IGenericRepository genericRepo)
        {
            _genericRepo = genericRepo;
        }

        public async Task<bool> CreateClientAppointment(CreateClientAppointment clientAppointment)
        {
            var apt = _genericRepo.GetAllReadOnly<AppointmentEntity>(apt => apt.AppointmentDate == clientAppointment.AppointmentDate);
            var isAvailable = !apt.Any(apt => IsNotValidAppointmentInterval(clientAppointment.AppointmentStartsAt, apt.AppointmentEndsAt, clientAppointment.AppointmentEndsAt, apt.AppointmentStartsAt));

            return isAvailable;
        }

        private bool IsNotValidAppointmentInterval(TimeOnly requestedAppointmentStartsAt, TimeOnly existingAppointmentEndsAt,
            TimeOnly requestedAppointmentEndsAt, TimeOnly existingAppointmentStartsAt)
        {
            return requestedAppointmentStartsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt) 
                || requestedAppointmentEndsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt);
        }
    }
}
