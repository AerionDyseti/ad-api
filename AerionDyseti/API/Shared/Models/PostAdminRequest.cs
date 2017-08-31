using System.ComponentModel.DataAnnotations;

namespace AerionDyseti.API.Shared.Models
{
    /// <summary>
    /// DTO representing a request to make the user with the given Email an Admin.
    /// </summary>
    public class PostAdminRequest
    {
        /// <summary>
        /// The email for the user to be made or removed as an Admin (granted or revoked an AdminClaim).
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The new value to set whether or not this user is an Admin.
        /// </summary>
        [Required]
        public bool Admin { get; set; }
    }
}
