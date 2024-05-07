using API_EF.Helper;
using API_EF.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy =TypeSafe.Policy.FullPolicy)]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager; 
        }


        [HttpGet]
        [Authorize(Roles =TypeSafe.Roles.Admin)]
        public async Task<IActionResult> GetListRole()
        {
            var listRole = await _roleManager.Roles.ToListAsync();
            return Ok(listRole);
        }


        [HttpPost]
        public async  Task<IActionResult> CreateRole(RoleDTOs role)
        {
            if (!ModelState.IsValid) return BadRequest("Don't have a Data ");
            bool checkRoleExits = await _roleManager.RoleExistsAsync(role.RoleName);
            if(checkRoleExits) return BadRequest("Had a role in Database");
            IdentityRole identityRole = new IdentityRole
            {
                Name = role.RoleName,
                NormalizedName = role.NormalizedName,
                ConcurrencyStamp = role.ConcurencyStamp
            };
            var resultAdd = await _roleManager.CreateAsync(identityRole);
            if(resultAdd.Succeeded) return Ok("CreatSuccess");
            return BadRequest(resultAdd.Errors.ToList()[0].Description);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string? nameRole)
        {
            if (!ModelState.IsValid) return BadRequest("Don't have datas");
            var getRole = await _roleManager.FindByNameAsync(nameRole);
            if (getRole == null) return BadRequest("Don't have role with name : " + nameRole);
            var result = await _roleManager.DeleteAsync(getRole);
            if (result.Succeeded) return Ok("Delete Done");
            return BadRequest(result.Errors.ToList()[0].Description);
        }
    }
}
