#nullable enable
using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.IO;
using System;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim([Bind("LecturerName,HoursWorked,HourlyRate")] Claim model, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                var claim = new Claim
                {
                    LecturerName = model.LecturerName,
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Status = "Pending"
                };

                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Path.GetFileName(uploadedFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                        await uploadedFile.CopyToAsync(fileStream);

                    claim.UploadedFiles = fileName;
                }

                _context.Add(claim);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Claim submitted successfully!";
                return RedirectToAction(nameof(SubmitClaim));
            }

            return View(model);
        }
    }
}