using System;

namespace VE.DataTransferObject.Entities
{
    public class AppProspectiveVendorMaterials
    {
        public int Id { get; set; }
        public int ProspectiveVendorId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string ExtraProperties { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LastModifierId { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string VendorCode { get; set; }
    }
}