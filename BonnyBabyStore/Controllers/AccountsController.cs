using Microsoft.AspNetCore.Mvc;
using BonnyBabyStore.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BonnyBabyStore.Controllers
{
    
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

     
        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.UsernameOrEmail || u.UserName == model.UsernameOrEmail);

                if (user != null && HashPassword(model.Password) == user.PasswordHash)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    if (user.Role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Redirect based on user role
                    if (user.Role != null && user.Role.Name == "Admin")
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة.");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

 
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email || u.UserName == model.Username))
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم أو البريد الإلكتروني موجود بالفعل.");
                    return View(model);
                }

            
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                var newUser = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    RoleId = role?.Id ?? 0,
                    FirstName = "",
                    LastName = "",
                    Age = 0,
                    Gender = "",
                    ImageUrl = ""
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Automatically log in the user after registration
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, newUser.UserName),
                    new Claim(ClaimTypes.Email, newUser.Email)
                };

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redirect based on user role after registration
                if (role != null && role.Name == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Accounts");
        }

  
        public IActionResult AccessDenied()
        {
            return View();
        }

   
    
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
