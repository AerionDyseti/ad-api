using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace AerionDyseti.API.Shared.Auth
{
    public static class AdminOnlyPolicy
    {
        public static string PolicyName { get; set; } = "AdminOnly";
        public static Claim AdminClaim { get; set; } = new AdminClaim();

        public static void ConfigurePolicy(AuthorizationPolicyBuilder policy)
        {
            policy.RequireClaim(AdminClaim.Type, AdminClaim.Value);
        }
    }
}
