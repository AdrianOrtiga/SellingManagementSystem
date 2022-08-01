using System;
using System.Collections.Generic;

namespace SellingManagementSystem.Models
{
    public partial class Concept
    {
        public long Id { get; set; }
        public long SellId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Sell Sell { get; set; } = null!;
    }
}
