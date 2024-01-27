namespace VE.DataTransferObject.Entities
{
    public class VendorEnlistment : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProductMaterial { get; set; }
        public string Description { get; set; }
        public string SupGenralReq { get; set; }
        public string SupOtherReq { get; set; }
        public string SupType { get; set; }
        public int ExistingSupCount { get; set; }
        public string ExistingSupProblem { get; set; }
        public string SupAddReason { get; set; }
        public string Status { get; set; }
        public string PendingWith { get; set; }
        public string VendorCode { get; set; }
    }
}