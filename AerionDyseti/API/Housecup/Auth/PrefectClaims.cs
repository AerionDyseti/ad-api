using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AerionDyseti.API.Housecup.Auth
{
    public class PrefectClaim : Claim
    {
        public PrefectClaim(string house) : base("prefect", house) { }



    }


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
