using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BCrypt.Net;
using LabourInc_Inquiry_Systems.Models;
using LabourInc_Inquiry_Systems.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LabourInc_Inquiry_Systems.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var inquiries = await _db.Inquiries.OrderByDescending(i => i.SubmittedAt).ToListAsync();
            return View(inquiries);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var inquiry = await _db.Inquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            inquiry.Status = status;
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var inquiry = await _db.Inquiries.FindAsync(id);
            if (inquiry == null || string.IsNullOrEmpty(inquiry.AttachmentFileName))
                return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", inquiry.AttachmentFileName);
            var contentType = "application/octet-stream";
            return File(System.IO.File.OpenRead(filePath), contentType, inquiry.AttachmentFileName);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
    

}
