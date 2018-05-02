using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime AppointmentDatetime { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
