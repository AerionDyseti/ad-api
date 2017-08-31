using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti.API.Shared.Models
{
    /// <summary>
    /// Class representing an API Error, including a short code and description of the issue.
    /// </summary>
    public class ApiError
    {
        public string Code { get; set; }
        public string Description { get; set; }


        public ApiError()
        {
            this.Code = "UNKNOWN";
            this.Description = "An unknown error occurred.";
        }

        public ApiError(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

    }
}
