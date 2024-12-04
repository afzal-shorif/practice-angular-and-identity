using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos;
using UserManagement.Core.Entities;
using UserManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManagement;
        private readonly SignInManager<User> _signInManager;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly UserRoleService _userRoleService;
        // need mapper

        public UserController(UserManager<User> userManagement,
                              SignInManager<User> signInManager,
                              UserService userService,
                              RoleService roleService, 
                              UserRoleService userRoleService) 
        { 
            _userManagement = userManagement;
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegDto) // user dto
        {
            if(userRegDto == null)
            {
                return BadRequest("Invalid User Info");
            }

            var role = await _roleService.GetRoleAsync(userRegDto.roleId);

            if (role == null)
            {
                return BadRequest("Invalid Role Info");
            }

            // map the user dto object with user object

            var user = new User();
            user.Email = userRegDto.Email;
            user.FirstName = userRegDto.FirstName;
            user.LastName = userRegDto.LastName;
            user.UserName = userRegDto.UserName;
            user.isActive = false;

            var result = await _userManagement.CreateAsync(user, userRegDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("An error occur while register new user. Please try later.");
            }

            await _userRoleService.AddUserRole(user, role);

            return Ok(result);
        }

        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {                 
            // if the user role is admin, send the user list
            // otherwise send empty list
            ClaimsPrincipal currentUser = this.User;


            //var users = await _userManagement.Users.ToListAsync<User>();
            //var users = await _userService.GetUsersAsync();
            var users = await _userManagement.Users.Select(u => new UserListResponsDto{
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.isActive
            }).ToListAsync();

            return Ok(users);
            //return null;
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateActiveStatus(int userId, string status)
        {
            // if the login user is admin, active the user
            // otherwise send and error response

            return null;
        }

        [HttpPost("/logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] object empty)
        {
            if (empty != null)
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }

            return Unauthorized();
        }
    }
}
