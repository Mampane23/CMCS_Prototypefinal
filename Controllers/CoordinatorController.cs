using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS.Data;
using CMCS.Models;
using System.Text;

namespace CMCS.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coordinator Dashboard
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

        // GET: Coordinator/PendingClaims
        public async Task<IActionResult> PendingClaims()
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .OrderBy(c => c.SubmissionDate)
                .ToListAsync();

            return View(pendingClaims);
        }

        // GET: Coordinator/Review/5
        public async Task<IActionResult> Review(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);

            if (claim == null)
            {
                return NotFound();
            }

            // Automated validation checks
            ViewBag.ValidationResults = AutomatedValidation(claim);

            return View(claim);
        }

        // POST: Coordinator/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string? comments)
        {
            var claim = await _context.Claims.FindAsync(id);

            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Approved";
            claim.ReviewedBy = "Programme Coordinator";
            claim.ReviewedDate = DateTime.Now;
            claim.Comments = comments;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Claim #{claim.Id} has been approved successfully!";
            return RedirectToAction(nameof(PendingClaims));
        }

        // POST: Coordinator/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? comments)
        {
            if (string.IsNullOrWhiteSpace(comments))
            {
                TempData["ErrorMessage"] = "Comments are required when rejecting a claim.";
                return RedirectToAction(nameof(Review), new { id });
            }

            var claim = await _context.Claims.FindAsync(id);

            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Rejected";
            claim.ReviewedBy = "Programme Coordinator";
            claim.ReviewedDate = DateTime.Now;
            claim.Comments = comments;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Claim #{claim.Id} has been rejected.";
            return RedirectToAction(nameof(PendingClaims));
        }

        // GET: Coordinator/ApprovedClaims
        public async Task<IActionResult> ApprovedClaims()
        {
            var approvedClaims = await _context.Claims
                .Where(c => c.Status == "Approved")
                .OrderByDescending(c => c.ReviewedDate)
                .ToListAsync();

            return View(approvedClaims);
        }

        // GET: Coordinator/RejectedClaims
        public async Task<IActionResult> RejectedClaims()
        {
            var rejectedClaims = await _context.Claims
                .Where(c => c.Status == "Rejected")
                .OrderByDescending(c => c.ReviewedDate)
                .ToListAsync();

            return View(rejectedClaims);
        }

        // Automated validation logic
        private Dictionary<string, string> AutomatedValidation(Claim claim)
        {
            var results = new Dictionary<string, string>();

            // Check 1: Hours worked validation
            if (claim.HoursWorked < 1 || claim.HoursWorked > 744)
            {
                results.Add("Hours Validation", "⚠️ Warning: Hours worked is outside normal range (1-744)");
            }
            else if (claim.HoursWorked > 200)
            {
                results.Add("Hours Validation", "⚠️ Warning: Hours worked exceeds 200 hours - requires additional verification");
            }
            else
            {
                results.Add("Hours Validation", "✅ Pass: Hours worked is within acceptable range");
            }

            // Check 2: Hourly rate validation
            if (claim.HourlyRate < 50 || claim.HourlyRate > 1000)
            {
                results.Add("Rate Validation", "⚠️ Warning: Hourly rate is outside standard range (R50-R1000)");
            }
            else if (claim.HourlyRate > 500)
            {
                results.Add("Rate Validation", "⚠️ Warning: Hourly rate exceeds R500 - requires justification");
            }
            else
            {
                results.Add("Rate Validation", "✅ Pass: Hourly rate is within acceptable range");
            }

            // Check 3: Total amount validation
            if (claim.TotalAmount > 100000)
            {
                results.Add("Amount Validation", "⚠️ Warning: Total amount exceeds R100,000 - requires senior approval");
            }
            else if (claim.TotalAmount > 50000)
            {
                results.Add("Amount Validation", "⚠️ Caution: Total amount exceeds R50,000");
            }
            else
            {
                results.Add("Amount Validation", "✅ Pass: Total amount is within normal range");
            }

            // Check 4: Document upload validation
            if (string.IsNullOrEmpty(claim.UploadedFiles))
            {
                results.Add("Document Validation", "⚠️ Warning: No supporting documents uploaded");
            }
            else
            {
                results.Add("Document Validation", "✅ Pass: Supporting documents provided");
            }

            // Check 5: Submission date validation
            var daysSinceSubmission = (DateTime.Now - claim.SubmissionDate).Days;
            if (daysSinceSubmission > 30)
            {
                results.Add("Timeline Validation", $"⚠️ Warning: Claim is {daysSinceSubmission} days old");
            }
            else
            {
                results.Add("Timeline Validation", "✅ Pass: Claim submitted within reasonable timeframe");
            }

            // Check 6: Required fields validation
            var missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(claim.LecturerName)) missingFields.Add("Lecturer Name");
            if (string.IsNullOrWhiteSpace(claim.Department)) missingFields.Add("Department");
            if (string.IsNullOrWhiteSpace(claim.ModuleCode)) missingFields.Add("Module Code");

            if (missingFields.Any())
            {
                results.Add("Completeness Check", $"⚠️ Warning: Missing fields - {string.Join(", ", missingFields)}");
            }
            else
            {
                results.Add("Completeness Check", "✅ Pass: All required fields completed");
            }

            return results;

        }
        // GET: Coordinator/ExportToCSV
public async Task<IActionResult> ExportToCSV(string? status)
{
    var query = _context.Claims.AsQueryable();

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(c => c.Status == status);
    }

    var claims = await query.OrderByDescending(c => c.SubmissionDate).ToListAsync();

    var csv = new StringBuilder();
    csv.AppendLine("Claim ID,Lecturer Name,Department,Module Code,Hours Worked,Hourly Rate,Total Amount,Status,Submission Date,Reviewed By,Review Date,Comments");

    foreach (var claim in claims)
    {
        csv.AppendLine($"{claim.Id}," +
                      $"\"{claim.LecturerName}\"," +
                      $"\"{claim.Department ?? "N/A"}\"," +
                      $"\"{claim.ModuleCode ?? "N/A"}\"," +
                      $"{claim.HoursWorked}," +
                      $"{claim.HourlyRate}," +
                      $"{claim.TotalAmount}," +
                      $"{claim.Status}," +
                      $"{claim.SubmissionDate:yyyy-MM-dd}," +
                      $"\"{claim.ReviewedBy ?? "N/A"}\"," +
                      $"{(claim.ReviewedDate.HasValue ? claim.ReviewedDate.Value.ToString("yyyy-MM-dd") : "N/A")}," +
                      $"\"{claim.Comments ?? "N/A"}\"");
    }

    var fileName = $"Claims_{status ?? "All"}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
    return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
}
    }
}