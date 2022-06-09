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
    }
}
