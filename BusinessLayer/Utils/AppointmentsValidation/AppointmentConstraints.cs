namespace AppointmentsManagerMs.BusinessLayer.Utils.AppointmentsValidation
{
    public class AppointmentConstraints
    {
        public static bool IsNotValidAppointmentInterval(TimeOnly requestedAppointmentStartsAt, TimeOnly existingAppointmentEndsAt,
    TimeOnly requestedAppointmentEndsAt, TimeOnly existingAppointmentStartsAt)
        {
            return requestedAppointmentStartsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt)
                || requestedAppointmentEndsAt.IsBetween(existingAppointmentStartsAt, existingAppointmentEndsAt);
        }
    }
}
