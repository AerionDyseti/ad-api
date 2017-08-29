using AerionDyseti.Auth.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AerionDyseti.Extensions
{
    public static class SignInManagerExtensions
    {

        public static async Task<SignInResult> CheckPasswordSignIn(this SignInManager<AerionDysetiUser> mgr, AerionDysetiUser user, string password)
        {
            return await mgr.CheckPasswordSignInAsync(user, password, false);
        }

        public static async Task SignIn(this SignInManager<AerionDysetiUser> mgr, AerionDysetiUser user)
        {
            await mgr.SignInAsync(user, false);
        }

    }
}
