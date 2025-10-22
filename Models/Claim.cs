#nullable enable
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public string? LecturerName { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal HourlyRate { get; set; }

        public decimal TotalAmount => HoursWorked * HourlyRate; // read-only, auto-calculated

        public string Status { get; set; } = "Pending";

        public string? UploadedFiles { get; set; }
    }
}
