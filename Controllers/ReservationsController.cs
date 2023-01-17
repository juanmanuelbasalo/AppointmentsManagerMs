using AppointmentsManagerMs.BusinessLayer.Models;
using AppointmentsManagerMs.BusinessLayer.Models.Responses;
using AppointmentsManagerMs.BusinessLayer.Services.ManagementService;
using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.BusinessLayer.StateMachines.Events;
using AppointmentsManagerMs.BusinessLayer.Utils.AppointmentsValidation;
using AppointmentsManagerMs.DataAccessLayer;
using AppointmentsManagerMs.DataAccessLayer.Repository;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManagerMs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IGenericRepository<SagaAppointmentsManagerContext> _repo;
        private readonly IAppointmentManagerService _service;

        public ReservationsController(ILogger<ReservationsController> logger,
            IPublishEndpoint publishEndpoint,
            IGenericRepository<SagaAppointmentsManagerContext> repo, 
            IAppointmentManagerService service)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _repo = repo;
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RequestReservation([FromBody] RequestAppointmentReservation reserve)
        {
            try
            {
                var reservationId = NewId.NextGuid();

                var appointment = await _repo.FindAsync<Appointment>(x => x.CorrelationId == reserve.AppointmentId && x.CurrentState == 3);

                if (appointment is null)
                {
                    return BadRequest($"Sorry that appointmen is already taken, please try with another date or time.");
                }

                await _publishEndpoint.Publish(new AppointmentRequested
                { 
                    ReservationId = reservationId,
                    AppointmentId = reserve.AppointmentId,
                    Client = new AppointmentUser 
                    {
                        DocumentId = reserve.DocumentId,
                        LastName = reserve.LastName,
                        Name = reserve.Name,
                        UserEmail = reserve.UserEmail,
                        UserPhone = reserve.UserPhone
                    },
                    StartsAt = appointment.AppointmentStartsAt,
                    Date = appointment.AppointmentDate,
                    Duration = appointment.AppointmentDuration
                });

                return Accepted("api/reservations/{id}", reservationId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/approve")]
        [ProducesResponseType(202)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ApproveReservation(Guid id)
        {
            try
            {
                var reservation = await _repo.FindAsync<Reservation>(x => x.CorrelationId == id && x.CurrentState == 3);

                if (reservation is null)
                {
                    return BadRequest($"Sorry that appointment is already taken, please try with another date or time.");
                }

                await _publishEndpoint.Publish(new AppointmentAccepted
                {
                    ReservationId = id
                });

                return Accepted("api/reservations/{id}", id);
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
        public async Task<ActionResult> CancelReservation(Guid id)
        {
            try
            {
                await _publishEndpoint.Publish(new ReservationCanceled
                {
                    ReservationId = id
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
        public ActionResult<IEnumerable<ReservationResponse>> ReturnReservations()
        {
            try
            {
                var reservations = _repo.GetAllReadOnly<Reservation>().ToArray();

                var response = reservations.Select(x => new ReservationResponse
                {
                    AppointmentDate = x.Date,
                    AppointmentStartsAt = x.StartsAt,
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
        public async Task<ActionResult<ReservationResponse>> ReturnReservationById(Guid id)
        {
            try
            {
                var reservation = await _repo.FindAsync<Reservation>(x => x.CorrelationId == id);

                if (reservation is null)
                    return NotFound();

                return new ReservationResponse
                {
                    AppointmentDate = reservation.Date,
                    AppointmentStartsAt = reservation.StartsAt,
                    CurrentStatus = reservation.CurrentState.ToString(),
                    Id = reservation.CorrelationId
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
