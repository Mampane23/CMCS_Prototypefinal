using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer name is required")]
        [Display(Name = "Lecturer Name")]
        [StringLength(100)]
        public string LecturerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours worked is required")]
        [Range(1, 744, ErrorMessage = "Hours must be between 1 and 744 (max hours in a month)")]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(50, 1000, ErrorMessage = "Hourly rate must be between R50 and R1000")]
        [Display(Name = "Hourly Rate (ZAR)")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        [Display(Name = "Supporting Documents")]
        public string? UploadedFiles { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Reviewed By")]
        public string? ReviewedBy { get; set; }

        [Display(Name = "Review Date")]
        public DateTime? ReviewedDate { get; set; }

        [Display(Name = "Reviewer Comments")]
        [StringLength(500)]
        public string? Comments { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? LecturerEmail { get; set; }

        [Display(Name = "Department")]
        [StringLength(100)]
        public string? Department { get; set; }

        [Display(Name = "Module Code")]
        [StringLength(20)]
        public string? ModuleCode { get; set; }

        [Display(Name = "Month")]
        public string? ClaimMonth { get; set; }
    }
}
