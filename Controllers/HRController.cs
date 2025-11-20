using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS.Data;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims.ToListAsync();

            var approvedClaims = claims.Where(c => c.Status == "Approved").ToList();
            var totalPayable = approvedClaims.Sum(c => c.TotalAmount);

            ViewBag.TotalClaims = claims.Count;
            ViewBag.ApprovedClaims = approvedClaims.Count;
            ViewBag.TotalPayable = totalPayable;
            ViewBag.PendingPayment = approvedClaims.Count;

            return View(claims);
        }
        public IActionResult ApprovedClaims()
{
    return View();
}

public IActionResult GenerateReport()
{
    return View();
}

public IActionResult LecturerManagement()
{
    return View();
}

public IActionResult Statistics()
{
    return View();
}

// GET: HR/GenerateInvoice/5
public async Task<IActionResult> GenerateInvoice(int? id)
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

    if (claim.Status != "Approved")
    {
        TempData["ErrorMessage"] = "Can only generate invoices for approved claims.";
        return RedirectToAction(nameof(ApprovedClaims));
    }

    return View(claim);
}

// GET: HR/ProcessPayment/5
public async Task<IActionResult> ProcessPayment(int? id)
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

    if (claim.Status != "Approved")
    {
        TempData["ErrorMessage"] = "Can only process payment for approved claims.";
        return RedirectToAction(nameof(ApprovedClaims));
    }

    return View(claim);
}

// POST: HR/ConfirmPayment/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ConfirmPayment(int id)
{
    var claim = await _context.Claims.FindAsync(id);

    if (claim == null)
    {
        return NotFound();
    }

    // In a real system, you would update payment status here
    TempData["SuccessMessage"] = $"Payment of R{claim.TotalAmount:N2} processed for {claim.LecturerName}";
    
    return RedirectToAction(nameof(ApprovedClaims));
}

    }

    
}