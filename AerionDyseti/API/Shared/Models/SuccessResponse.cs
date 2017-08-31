using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti.API.Shared.Models
{
    /// <summary>
    /// Response when all we need to do is signal that whatever action we took succeeeded.
    /// </summary>
    public class SuccessResponse
    {
        public bool Success { get; set; } = true;
    }
}
