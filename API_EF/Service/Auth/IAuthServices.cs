using API_EF.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections;

namespace API_EF.Service.Auth
{
    public interface IAuthServices
    {
        Task<string> GenerateTokenString(AccountUsers user,JwtConfiguration configuration);
        Task<bool> Login(AccountUsers user);
        Task<IdentityResult> RegisterUser(AccountUsers user);
        Task<bool> AddUserClaim(string user, Claim claim);
        Task GenerateCookieAuthentication(string username);
        Task Logout();
        Task<List<Claim>> GetClaims(string username);
        Task<bool> AddRoleForUser(string username);



    }
}
