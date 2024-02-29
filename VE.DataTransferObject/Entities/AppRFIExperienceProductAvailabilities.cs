using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class AppRFIExperienceProductAvailabilities
    {
        public int Id { get; set; }
        public string ProductionVolume { get; set; }
        public int BusinessYear { get; set; }
        public double StorageCapacity { get; set; }
        public string PreviousCompanyReference { get; set; }
        public string CompanyBrochureFileName { get; set; }
        public int ProspectiveVendorId { get; set; }
    }
}
