using System;
using System.Collections.Generic;

namespace FYPHomecare.Models
{
    public partial class Quote
    {
        public int QuoteId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ServiceProviderId { get; set; }
        public int UsersId { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual Users Users { get; set; }
    }
}
