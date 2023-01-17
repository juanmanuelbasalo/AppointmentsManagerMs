using AppointmentsManagerMs.BusinessLayer.Models;

namespace AppointmentsManagerMs.BusinessLayer.Services.ManagementService
{
    public interface IAppointmentManagerService
    {
        bool IsTimeSlotTaken(DateOnly appointmentDate, TimeOnly requestedAppointmentStartsAt, TimeOnly requestedAppointmentEndsAt);
    }
}
