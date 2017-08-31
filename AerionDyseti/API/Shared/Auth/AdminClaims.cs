using System.Security.Claims;

namespace AerionDyseti.API.Shared.Auth
{
    /// <inheritdoc />
    /// <summary>
    ///     Claim to be an Administrator, granting all rights and privileges for all areas in the API.
    /// </summary>
    public class AdminClaim : Claim
    {

        public AdminClaim() : base("admin", "true")
        {
        }
    }
}