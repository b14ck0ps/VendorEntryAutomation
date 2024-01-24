using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class AppVendorEnlistmentLogs
    {
        public int Id { get; set; }
        public int ProspectiveVendorId { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string Action { get; set; }
        public string ActionById { get; set; }
        public string Comment { get; set; }
        public string ExtraProperties { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LastModifierId { get; set; }
    }
}
