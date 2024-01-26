using System;

namespace VE.DataTransferObject.Entities
{
    public class AppProspectiveVendors
    {
        public int ProspectiveVendorId { get; set; }
        public string Code { get; set; }
        public int RequestorID { get; set; }
        public string ServiceDescription { get; set; }
        public string RequirementGeneral { get; set; }
        public string RequirementOther { get; set; }
        public int TypeOfSupplierId { get; set; }
        public int ExisitngSupplierCount { get; set; }
        public string ExisitngSupplierProblem { get; set; }
        public string NewSupplierAdditionReason { get; set; }
        public string VendorName { get; set; }
        public string VendorEmail { get; set; }
        public int Status { get; set; }
        public string ExtraProperties { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LastModifierId { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string PendingWithUserId { get; set; }
        public bool IsIncludedIntoSAP { get; set; }
        public string SAPVendorCode { get; set; }
        public float? ScoreCard { get; set; }
    }
}