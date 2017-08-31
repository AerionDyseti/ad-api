using System;
using Microsoft.AspNetCore.Identity;

namespace AerionDyseti.API.Shared.Models
{
    public class AerionDysetiUser : IdentityUser
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}