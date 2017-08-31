using System.ComponentModel.DataAnnotations;

namespace AerionDyseti.API.Shared.Models
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}