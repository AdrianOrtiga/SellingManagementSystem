using System;
using System.Collections.Generic;

namespace SellingManagementSystem.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
