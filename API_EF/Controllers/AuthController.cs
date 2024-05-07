using API_EF.Helper;
using API_EF.Models;
using API_EF.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthServices _authServices;
        private readonly JwtConfiguration _jwtConfiguration;
        public AuthController(IAuthServices authServices, JwtConfiguration jwtconfig)
        {
            _authServices = authServices;
            _jwtConfiguration = jwtconfig;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountUsers account)
        {
            var result = await _authServices.RegisterUser(account);
            if (result.Succeeded) return Ok("SuccessFull");
            string erros = string.Empty;
            foreach(var error  in result.Errors)
            {
                erros += error.Description + "   ";
            }
            return BadRequest(erros);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountUsers account)
        {
            var result = await _authServices.Login(account);
            if (result)
            {
               // await _authServices.AddRoleForUser(account.EmailLogin);
                var stringtoken = await _authServices.GenerateTokenString(account, _jwtConfiguration);
                await _authServices.GenerateCookieAuthentication(account.EmailLogin);
                return Ok(stringtoken);
            }
            return BadRequest("Login fail");
        }
    }
}
