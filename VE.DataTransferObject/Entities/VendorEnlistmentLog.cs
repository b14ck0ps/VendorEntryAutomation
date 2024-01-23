using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class VendorEnlistmentLog : BaseEntity
    {
        public string VendorCode { get; set; }
        public string Status { get; set; }
        public string ActionBy { get; set; }
        public string Comment { get; set; }

    }
}
