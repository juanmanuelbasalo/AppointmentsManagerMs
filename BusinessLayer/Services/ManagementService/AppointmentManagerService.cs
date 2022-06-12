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
            var appointments = _genericRepo.GetAllReadOnly<AppointmentEntity>(apt => apt.AppointmentDate == clientAppointment.AppointmentDate);
            var isAlreadyTaken = appointments.Any(apt => IsNotValidAppointmentInterval(clientAppointment.AppointmentStartsAt, apt.AppointmentEndsAt, 
                clientAppointment.AppointmentEndsAt, apt.AppointmentStartsAt));

            if (isAlreadyTaken)
                return false;

            AppointmentEntity appointment = new()
            {
                AppointmentDate = clientAppointment.AppointmentDate,
                AppointmentDuration = clientAppointment.AppointmentDuration,
                AppointmentStartsAt = clientAppointment.AppointmentStartsAt,
                ClientEmail = clientAppointment.Client.UserEmail,
                ClientPhone = clientAppointment.Client.UserPhone,
                ClientFirstName = clientAppointment.Client.Name,
                ClientLastName = clientAppointment.Client.LastName,
                DoctorOfficeId = clientAppointment.DoctorOfficeId,
            };

            _genericRepo.Insert(appointment);

            var isCreated = await _genericRepo.SaveAsync(clientAppointment.Client.UserEmail);

            return isCreated;
        }

        public bool IsNotValidAppointmentInterval(TimeOnly requestedAppointmentStartsAt, TimeOnly existingAppointmentEndsAt,
            TimeOnly requestedAppointmentEndsAt, TimeOnly existingAppointmentStartsAt)
        {
            return requestedAppointmentStartsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt) 
                || requestedAppointmentEndsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt);
        }
    }
}
