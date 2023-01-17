using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.Models.Responses;
using AppointmentsManagerMs.BusinessLayer.Services.ManagementService;
using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.BusinessLayer.StateMachines.Events;
using AppointmentsManagerMs.DataAccessLayer;
using AppointmentsManagerMs.DataAccessLayer.Repository;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManagerMs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IGenericRepository<SagaAppointmentsManagerContext> _repo;
        private readonly IAppointmentManagerService _service;

        public AppointmentsController(ILogger<AppointmentsController> logger,
            IPublishEndpoint publishEndpoint,
            IGenericRepository<SagaAppointmentsManagerContext> repo, 
            IAppointmentManagerService service)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _repo = repo;
            _service = service;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointment appointmentUpdate)
        {
            var date = DateOnly.FromDateTime(appointmentUpdate.Date);
            var startsAt = TimeOnly.FromDateTime(appointmentUpdate.Date);
            var endsAt = TimeOnly.FromDateTime(appointmentUpdate.Date).Add(appointmentUpdate.Duration);

            try
            {
                var appointment = await _repo.FindAsync<Appointment>(x => x.CorrelationId == id);

                if (appointment is null)
                    return NotFound();

                if (_service.IsTimeSlotTaken(date, startsAt, endsAt))
                {
                    return UnprocessableEntity("There is another appointment in that time slot. Either delete the other one or assign another time slot.");
                }

                await _publishEndpoint.Publish(new AppointmentUpdated
                {
                    AppointmentId = id,
                    Date = date,
                    Duration = appointmentUpdate.Duration,
                    StartsAt = startsAt,
                    DoctorOfficeId = appointmentUpdate.DoctorOfficeId
                });

                return Accepted("api/appointments/{id}", id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateAppointment([FromBody] CreateAppointment appointment)
        {
            var startsAt = TimeOnly.FromDateTime(appointment.Date);
            var endsAt = TimeOnly.FromDateTime(appointment.Date).Add(appointment.Duration);
            var date = DateOnly.FromDateTime(appointment.Date);

            try
            {
                var appointmentId = NewId.NextGuid();

                var isAlreadyTaken = _service.IsTimeSlotTaken(date, startsAt, endsAt);

                if (isAlreadyTaken)
                {
                    return BadRequest("There is another appointment in that time slot. Either delete the other one or assign another time slot.");
                }

                await _publishEndpoint.Publish(new AppointmentAdded
                { 
                    AppointmentId = appointmentId,
                    Date = date,
                    Duration = appointment.Duration,
                    StartsAt = startsAt,
                    DoctorOfficeId = appointment.DoctorOfficeId,
                    DoctorId = appointment.DoctorId
                });

                return Accepted("api/appointments/{id}", appointmentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RemoveAppointment(Guid id)
        {
            try
            {
                await _publishEndpoint.Publish(new AppointmentRemoved
                {
                    AppointmentId = id
                });

                return Accepted("api/appointments/{id}", id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<AppointmentResponse>> ReturnAppointments()
        {
            try
            {
                var appointments = _repo.GetAllReadOnly<Appointment>().ToArray();

                var response = appointments.Select(x => new AppointmentResponse
                {
                    AppointmentDate = x.AppointmentDate,
                    AppointmentDuration = x.AppointmentDuration,
                    AppointmentStartsAt = x.AppointmentStartsAt,
                    CurrentStatus = x.CurrentState.ToString(),
                    Id = x.CorrelationId
                });

                return response.ToArray();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentResponse>> ReturnAppointmentById(Guid id)
        {
            try
            {
                var appointment = await _repo.FindAsync<Appointment>(x => x.CorrelationId == id);

                if (appointment is null)
                    return NotFound();

                return new AppointmentResponse 
                {
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentDuration = appointment.AppointmentDuration,
                    AppointmentStartsAt = appointment.AppointmentStartsAt,
                    CurrentStatus = appointment.CurrentState.ToString(),
                    Id = appointment.CorrelationId
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
