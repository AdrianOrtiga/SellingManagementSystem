using System;
using System.Collections.Generic;

namespace SellingManagementSystem.Models
{
    public partial class Client
    {
        public Client()
        {
            Sells = new HashSet<Sell>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Sell> Sells { get; set; }
    }
}
