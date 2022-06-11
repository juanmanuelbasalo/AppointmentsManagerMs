using AppointmentsManagerMs.BusinessLayer.Models;

namespace AppointmentsManagerMs.BusinessLayer.Services.ManagementService
{
    public interface IAppointmentManagerService
    {
        Task<bool> CreateClientAppointment(CreateClientAppointment clientAppointment);
    }
}
