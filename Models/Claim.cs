using System;

namespace CMCS_Prototype.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public int LecturerId { get; set; }
        public double Amount { get; set; }
        public DateTime DateSubmitted { get; set; }

        // Navigation property (nullable to avoid CS8618 warning)
        public Lecturer? Lecturer { get; set; }
    }
}
