using System.Collections.Generic;

namespace AerionDyseti.API.Shared.Models
{
    /// <summary>
    /// Response DTO for a request to fetch the list of Admins.
    /// </summary>
    public class GetAdminsResponse
    {
        // The list of emails for users that are set as Admins.
        public List<string> Admins { get; set; }


    }
}
