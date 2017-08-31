using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti.API.Shared.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
