using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System.Data;

namespace Shop.Controllers.API
{
    // This controller was created for our partners
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public APIUserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            Console.WriteLine("I'm here!!");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email и пароль обязательны.");
            }

            var user = new IdentityUser 
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok("User register successfully ...");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Auth(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email и пароль обязательны.");
            }
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: false,
                lockoutOnFailure: false
                );
            if (result.Succeeded)
            {
                return Ok("Authed successfully ...");
            }
            return BadRequest("Invalid email or password ...");
        }
        [Authorize(Roles = "admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update(string id, IdentityUser user)
        {
            if (user == null)
            {
                return BadRequest("User object is null.");
            }

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                return Ok(existingUser);
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            var deleted = await _userManager.DeleteAsync(existingUser);
            if (deleted.Succeeded)
            {
                return Ok(new { message = $"{existingUser.UserName} deleted successfully." });
            }
            return BadRequest(deleted.Errors);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create_role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is important ...");
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role already exists ...");
            }
            if (User.Identity.IsAuthenticated)
            {
                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok($"The role: {role.Name} is created ...");
                }

                return BadRequest(result.Errors);
            }
            return BadRequest("Role create error ...");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("assign_role")]
        public async Task<IActionResult> AssignRole(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Id or Name Role are important ...");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("The user not found ...");
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return NotFound($"The role name {roleName} not found");
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role: {roleName} assigned to the user with id: {id} ...");
            }
            return BadRequest(result.Errors);
        }

        public async Task<IActionResult> AccessDenied()
        {
            return BadRequest("Cookie Access Denied ...");
        }
    }
}