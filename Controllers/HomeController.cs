using Microsoft.AspNetCore.Mvc;
using CMCS_Prototype.Models;
using System.Collections.Generic;

namespace CMCS_Prototype.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Dummy data
            var lecturer = new Lecturer
            {
                LecturerId = 1,
                Name = "John Doe",
                Email = "john.doe@university.ac.za"
            };

            var claim = new Claim
            {
                ClaimId = 101,
                LecturerId = lecturer.LecturerId,
                Amount = 2500.00,
                DateSubmitted = System.DateTime.Now,
                Lecturer = lecturer
            };

            var approval = new ClaimApproval
            {
                ApprovalId = 5001,
                ClaimId = claim.ClaimId,
                Approved = true,
                ApproverName = "Admin User",
                Claim = claim
            };

            // Pass data to view
            ViewData["Lecturer"] = lecturer;
            ViewData["Claim"] = claim;
            ViewData["Approval"] = approval;

            return View();
        }
    }
}
