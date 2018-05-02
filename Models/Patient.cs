using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class Patient
    {
        public Patient()
        {
            ServiceRequest = new HashSet<ServiceRequest>();
        }

        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nric { get; set; }
        public string MedicalHistory { get; set; }
        public decimal AnnualIncome { get; set; }
        public int UsersId { get; set; }
        public string Address { get; set; }
        public string RelationshipToUser { get; set; }
        public int Postalcode { get; set; }
        public int Phone { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequest { get; set; }
    }
}
