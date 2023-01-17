namespace AppointmentsManagerMs.BusinessLayer.Models.Responses
{
    public record AppointmentResponse
    {
        private string _status;

        public Guid Id { get; init; }
        public string CurrentStatus { get => _status; init => _status = GetCurrentStateInString(value); }
        public DateOnly AppointmentDate { get; init; }
        public TimeOnly AppointmentStartsAt { get; init; }
        public TimeSpan AppointmentDuration { get; init; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }

        private static string GetCurrentStateInString(string currentState) => currentState switch
        {
            "3" => "Available",
            "4" => "Reserved",
            "2" => "Removed",
            _ => "Excluded",
        };
    }
}
