using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class ClaimSubmissionViewModel
    {
        [Required]
        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Hours Worked")]
        public int HoursWorked { get; set; }

        [Required]
        [Display(Name = "Hourly Rate (R)")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Supporting Documents")]
        public List<IFormFile> SupportingDocuments { get; set; } = new List<IFormFile>();

        // ✅ Automatically calculates total claim amount
        public decimal TotalAmount => HoursWorked * HourlyRate;
    }
}
