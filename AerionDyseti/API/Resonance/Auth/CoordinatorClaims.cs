using System.Security.Claims;

namespace AerionDyseti.API.Resonance.Auth
{
    /// <inheritdoc />
    /// <summary>
    ///     Abstract class representing a Coordinator claim.
    /// </summary>
    public abstract class CoordinatorClaim : Claim
    {
        protected CoordinatorClaim(string level) : base("Coordinator", level)
        {
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Claim to be a Domain Coordinator. Properties include the specific Domain code.
    /// </summary>
    public class DomainCoordinatorClaim : CoordinatorClaim
    {
        public DomainCoordinatorClaim(string domain) : base("Domain")
        {
            Properties.Add("Domain", domain);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Claim to be a Regional-level Coordinator. Properties include the specific Region code.
    /// </summary>
    public class RegionalCoordinatorClaim : CoordinatorClaim
    {
        public RegionalCoordinatorClaim(string region) : base("Regional")
        {
            Properties.Add("Region", region);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Claim to be a Organization-level (National) Coordinator. Properties include the specific Organization.
    /// </summary>
    public class NationalCoordinatorClaim : CoordinatorClaim
    {
        public NationalCoordinatorClaim(string club) : base("Organization")
        {
            Properties.Add("Organization", club);
        }
    }
}