using AppointmentsManagerMs.BusinessLayer.Models;

namespace AppointmentsManagerMs.BusinessLayer.Services.ManagementService
{
    public interface IAppointmentManagerService
    {
        Task<bool> CreateClientAppointmentAsync(CreateClientAppointment clientAppointment);
        bool IsNotValidAppointmentInterval(TimeOnly requestedAppointmentStartsAt, TimeOnly existingAppointmentEndsAt,
            TimeOnly requestedAppointmentEndsAt, TimeOnly existingAppointmentStartsAt);
    }
}
