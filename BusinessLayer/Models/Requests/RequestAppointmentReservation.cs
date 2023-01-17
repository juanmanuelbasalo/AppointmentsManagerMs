namespace AppointmentsManagerMs.BusinessLayer.Models
{
    public record RequestAppointmentReservation
    {
        public Guid AppointmentId { get; init; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? DocumentId { get; set; }
    }
}