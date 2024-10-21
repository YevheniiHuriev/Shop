using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if(string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is important ...");
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if(roleExists)
            {
                return BadRequest("Role already exists ...");
            }
            if(User.Identity.IsAuthenticated)
            {
                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok($"The role: {role.Name} is created ...");
                }

                return BadRequest(Json(result.Errors));
            }
            return BadRequest("Role create error ...");
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            ViewBag.Users = _userManager.Users.ToList();
            ViewBag.Roles = _roleManager.Roles.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string roleId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("User is not authenticated.");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (currentUser == null)
            {
                return BadRequest("Current user not found.");
            }

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            if (!currentUserRoles.Contains("admin"))
            {
                return BadRequest("You are not an admin.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null || role == null)
            {
                return BadRequest("User or role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return Ok($"The role: [{role.Name}] has been added to [{user.UserName}].");
            }

            return BadRequest("Error while adding the role.");
        }

        [HttpGet]
        public IActionResult AssignRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Id or Name Role are important ...");
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound("The user not found ...");
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if(!roleExist)
            {
                return NotFound($"The role name {roleName} not found");
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest(Json(result.Errors));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST-метод для создания нового пользователя
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
        {
            // Проверяем, что email и пароль были переданы
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // Если email или пароль отсутствуют, возвращаем ошибку
                return BadRequest("Email и пароль обязательны.");
            }

            // Создаем нового пользователя IdentityUser с указанным email
            var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };

            // Используем UserManager для создания пользователя с переданным паролем
            var result = await _userManager.CreateAsync(user, password);

            // Если создание прошло успешно
            if (result.Succeeded)
            {
                // Возвращаем успешный ответ с сообщением
                //return Ok("Пользователь создан.");
                return RedirectToAction("Index", "Home");
            }

            // Если возникли ошибки, добавляем их в ModelState для отображения пользователю
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Возвращаем ошибки, если не удалось создать пользователя
            return BadRequest(ModelState);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Проверяем, что email и пароль были переданы
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // Если email или пароль отсутствуют, возвращаем ошибку
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
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Email or Password are error ...");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
