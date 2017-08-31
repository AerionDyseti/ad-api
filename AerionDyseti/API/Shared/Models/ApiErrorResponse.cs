using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AerionDyseti.API.Shared.Models
{
    /// <summary>
    /// A list of Errors to be outputted by the API.
    /// </summary>
    public class ApiErrorResponse
    {
        public List<ApiError> Errors { get; set; }
    }
}
