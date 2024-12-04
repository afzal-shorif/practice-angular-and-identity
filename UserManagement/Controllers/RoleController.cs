using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Services;
using UserManagement.Core.Entities;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService) 
        {
            _roleService = roleService;
        }

        [HttpGet("/roles")]
        public async Task<IList<Role>>Get()
        {
            return await _roleService.GetRolesAsync();
        }
    }
}
