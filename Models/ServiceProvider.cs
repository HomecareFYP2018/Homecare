using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class ServiceProvider
    {
        public ServiceProvider()
        {
            Quote = new HashSet<Quote>();
            ServiceType = new HashSet<ServiceType>();
        }

        public int ServiceProviderId { get; set; }
        public int Phone { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int ExperienceYears { get; set; }
        public string Company { get; set; }
        public string Nric { get; set; }

        public virtual ICollection<Quote> Quote { get; set; }
        public virtual ICollection<ServiceType> ServiceType { get; set; }
    }
}
