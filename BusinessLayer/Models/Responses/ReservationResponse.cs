namespace AppointmentsManagerMs.BusinessLayer.Models.Responses
{
    public record ReservationResponse
    {
        private string _status = "";

        public Guid Id { get; init; }
        public string CurrentStatus { get => _status; init => _status = GetCurrentStateInString(value); }
        public DateOnly AppointmentDate { get; init; }
        public TimeOnly AppointmentStartsAt { get; init; }

        private static string GetCurrentStateInString(string currentState) => currentState switch
        {
            "3" => "Requested",
            "4" => "Accepted",
            "2" => "Removed",
            _ => "Excluded",
        };
    }
}
