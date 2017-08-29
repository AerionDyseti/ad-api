using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti.Auth.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string MesNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
