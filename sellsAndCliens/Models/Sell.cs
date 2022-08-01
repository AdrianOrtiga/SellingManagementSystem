using System;
using System.Collections.Generic;

namespace SellingManagementSystem.Models
{
    public partial class Sell
    {
        public Sell()
        {
            Concepts = new HashSet<Concept>();
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public decimal Total { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<Concept> Concepts { get; set; }
    }
}
