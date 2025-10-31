using Microsoft.AspNetCore.Mvc;
using LabourInc_Inquiry_Systems.Models;
using LabourInc_Inquiry_Systems.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LabourInc_Inquiry_Systems.Controllers
{
       
    public class InquiryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public InquiryController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public IActionResult Submit() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(InquiryFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var inquiry = new Inquiry
            {
                Name = model.Name,
                Company = model.Company,
                Email = model.Email,
                ServiceRequested = model.ServiceRequested,
                Details = model.Details
            };

            if (model.Attachment != null && model.Attachment.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Attachment.FileName)}";
                var path = Path.Combine(uploads, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.Attachment.CopyToAsync(stream);
                inquiry.AttachmentFileName = fileName;
            }

            _db.Inquiries.Add(inquiry);
            await _db.SaveChangesAsync();
            ViewBag.Message = "Inquiry submitted successfully!";
            ModelState.Clear();
            return View();
        }
    }
    

}
