﻿using AppointmentsManagerMs.BusinessLayer.Models;
using MassTransit;

namespace AppointmentsManagerMs.BusinessLayer.StateMachines
{
    public class Appointment : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentStartsAt { get; set; }
        public TimeSpan AppointmentDuration { get; set; }
        public TimeOnly AppointmentEndsAt { get => AppointmentStartsAt.Add(AppointmentDuration); }
        public Guid DoctorOfficeId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid? TokenExpirationId { get; set; }
        public DateTime? RemovedAt { get; set; }

        // If using Optimistic concurrency, this property is required
        public byte[]? RowVersion { get; set; }
    }
    
}
