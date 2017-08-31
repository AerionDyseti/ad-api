using System.Security.Claims;

namespace AerionDyseti.API.Resonance.Auth
{
    /// <summary>
    ///     Abstract base class for a Storyteller Claim.
    /// </summary>
    public abstract class StorytellerClaim : Claim
    {
        public StorytellerClaim(string level) : base("Storyteller", level)
        {
        }
    }

    /// <summary>
    ///     Claim to be a Venue Storyteller. Properties include the name of the Venue and the Domain Code.
    /// </summary>
    public class VenueStorytellerClaim : StorytellerClaim
    {
        public VenueStorytellerClaim(string venue, string domain) : base("Venue")
        {
            Properties.Add("Venue", venue);
            Properties.Add("Domain", domain);
        }
    }

    /// <summary>
    ///     Claim to be a Domain Storyteller. Properties include the specific Domain code.
    /// </summary>
    public class DomainStorytellerClaim : StorytellerClaim
    {
        public DomainStorytellerClaim(string domain) : base("Domain")
        {
            Properties.Add("Domain", domain);
        }
    }

    /// <summary>
    ///     Claim to be a Regional Storyteller. Properties include the specific Region code.
    /// </summary>
    public class RegionalStorytellerClaim : StorytellerClaim
    {
        public RegionalStorytellerClaim(string region) : base("Regional")
        {
            Properties.Add("Region", region);
        }
    }

    /// <summary>
    ///     Claim to be a Organization-Level (National) Storyteller. Properties include the name of the Organization.
    /// </summary>
    public class OrganizationStoryteller : StorytellerClaim
    {
        public OrganizationStoryteller(string club) : base("Organization")
        {
            Properties.Add("Organization", club);
        }
    }
}