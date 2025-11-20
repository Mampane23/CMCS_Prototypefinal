using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS.Data;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public LecturerController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Lecturer/Index
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();
            
            ViewBag.TotalClaims = claims.Count;
            ViewBag.PendingClaims = claims.Count(c => c.Status == "Pending");
            ViewBag.ApprovedClaims = claims.Count(c => c.Status == "Approved");
            ViewBag.RejectedClaims = claims.Count(c => c.Status == "Rejected");
            
            return View(claims);
        }

        // GET: Lecturer/Create
        public IActionResult Create()
        {
            var model = new Claim
            {
                SubmissionDate = DateTime.Now,
                ClaimMonth = DateTime.Now.ToString("MMMM yyyy")
            };
            return View(model);
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim, IFormFile? uploadedFile)
        {
            if (claim.HoursWorked <= 0)
            {
                ModelState.AddModelError("HoursWorked", "Hours worked must be greater than 0");
            }

            if (claim.HourlyRate <= 0)
            {
                ModelState.AddModelError("HourlyRate", "Hourly rate must be greater than 0");
            }

            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{uploadedFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    claim.UploadedFiles = uniqueFileName;
                }

                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;

                _context.Add(claim);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(claim);
        }
    }
}