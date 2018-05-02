using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class ServiceType
    {
        public int ServiceTypeId { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public int ServiceProviderId { get; set; }
        public int ExperienceYears { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
