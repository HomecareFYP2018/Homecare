using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class Users
    {
        public Users()
        {
            Quote = new HashSet<Quote>();
        }

        public int UsersId { get; set; }
        public string Nric { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Quote> Quote { get; set; }
    }
}
