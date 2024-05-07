using API_EF.Helper;
using API_EF.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace API_EF.Service.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       
  
        private readonly IHttpContextAccessor _httpContext;

        public AuthServices(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,  IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUser(AccountUsers user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.EmailLogin,
                Email = user.EmailLogin
            };
            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result;
        }

        public async Task<bool> Login(AccountUsers user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.EmailLogin);
            if (identityUser is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public async Task<string> GenerateTokenString(AccountUsers user , JwtConfiguration configuration)
        {
             /*var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Email,user.EmailLogin),
                 new Claim(ClaimTypes.Role,"Admin"),
             };*/


            var claims = await GetClaims(user.EmailLogin);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: configuration.Issuer,
                audience: configuration.Audience,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return  tokenString;
        }

        public async Task<bool> AddUserClaim(string user, Claim claim)
        {
            var identityUser = await _userManager.FindByNameAsync(user);
            if (identityUser is null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(identityUser, claim);
            return result.Succeeded;
        }

        public async Task GenerateCookieAuthentication(string username)
        {
            var claims = await GetClaims(username);

            ClaimsIdentity identity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal pricipal = new ClaimsPrincipal(identity);
            _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal);
        }
        public async Task Logout()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public async Task<List<Claim>> GetClaims(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),   
            };
            // var claimUser = await _userManager.GetClaimsAsync(user);
            //var claimUser = GetClaimsSeperated(await _userManager.GetClaimsAsync(user));

            // claims.AddRange(claimUser);
            claims.AddRange(GetClaimsSeperated(await _userManager.GetClaimsAsync(user)));
            var roles = await _userManager.GetRolesAsync(user);

             foreach (var role in roles)
             {
                 claims.Add(new Claim(ClaimTypes.Role, role));
                 var identityRole = await _roleManager.FindByNameAsync(role);
                 claims.AddRange(GetClaimsSeperated(await _roleManager.GetClaimsAsync(identityRole)));
            }

            return claims;
        }
        private List<Claim> GetClaimsSeperated(IList<Claim> claims)
        {
            var result = new List<Claim>();
            foreach (var claim in claims)
            {
                result.AddRange(claim.DeserializePermissions().Select(t => new Claim(claim.Type, t.ToString())));
            }
            return result;
        }

        public async Task<bool> AddRoleForUser(string username)
        {
            var user =  await _userManager.FindByEmailAsync(username);
            if (user == null) return false;
            var role = _roleManager.FindByNameAsync("Employee");
            var rolebyId = _roleManager.FindByIdAsync("88eb4fa4-7fe8-40c5-8bdb-d0a034a097b8");
            var result =  await _userManager.AddToRoleAsync(user, "Employee");
            return result.Succeeded;
           
        }
    }
}
