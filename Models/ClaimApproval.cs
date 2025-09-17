namespace CMCS_Prototype.Models
{
    public class ClaimApproval
    {
        public int ApprovalId { get; set; }
        public int ClaimId { get; set; }
        public bool Approved { get; set; }
        public string? ApproverName { get; set; }   // Nullable

        public Claim? Claim { get; set; }
    }
}
