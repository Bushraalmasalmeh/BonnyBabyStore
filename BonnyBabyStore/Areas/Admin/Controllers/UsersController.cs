using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BonnyBabyStore.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BonnyBabyStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            // Include Role to display the role name in the table
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return View(users);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            // Send list of roles to the view for the dropdown menu
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(User user, string password, IFormFile? imageFile)
        {
            ModelState.Remove("PasswordHash");
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string userImagesPath = Path.Combine(wwwRootPath, "images", "users");
                    if (!Directory.Exists(userImagesPath)) Directory.CreateDirectory(userImagesPath);

                    using (var fileStream = new FileStream(Path.Combine(userImagesPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    user.ImageUrl = "/images/users/" + fileName;
                }

                // Hash the password before saving
                if (!string.IsNullOrEmpty(password))
                {
                    user.PasswordHash = HashPassword(password);
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, string? newPassword, IFormFile? imageFile)
        {
            if (id != user.Id) return NotFound();

            // We remove password hash from model state because we handle it manually
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid)
            {
                var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

                // Handle image upload
                if (imageFile != null)
                {
                    if (!string.IsNullOrEmpty(userFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, userFromDb.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string userImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                    using (var fileStream = new FileStream(Path.Combine(userImagesPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    user.ImageUrl = "/images/users/" + fileName;
                }
                else
                {
                    // Keep the old image if no new one is uploaded
                    user.ImageUrl = userFromDb.ImageUrl;
                }

                // Handle password update
                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.PasswordHash = HashPassword(newPassword);
                }
                else
                {
                    // Keep the old password hash
                    user.PasswordHash = userFromDb.PasswordHash;
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "User updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "User deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }


        // Simple Hashing Method for demonstration
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }

}